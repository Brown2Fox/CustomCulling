using UnityEngine;
using System.Collections;

namespace RootMotion.Dynamics {
	// Handles switching BehaviourPuppet states.
	public partial class BehaviourPuppet : BehaviourBase {
		private Vector3 getUpPosition;
		private bool dropPropFlag;

		// Force the puppet to another state
		public void SetState(State newState) {
			// If already in this state, do nothing
			if (state == newState) return;

			Muscle[] muscles = puppetMaster.muscles;

			switch (newState) {
				// Switching to the PUPPET state
				case State.Puppet:
					puppetMaster.SampleTargetMappedState();
					unpinnedTimer = 0f;
					getUpTimer = 0f;
					hasCollidedSinceGetUp = false;
					//getupAnimationBlendWeight = 0f;
					//getupAnimationBlendWeightV = 0f;

					// If switching from the unpinned state...
					if (state == State.Unpinned) {
						// Repin the puppet
						for (var i = 0; i < muscles.Length; i++) {
							if (!muscles[i].state.isDisconnected) {
								muscles[i].state.pinWeightMlp = 1f;
								muscles[i].state.muscleWeightMlp = 1f;
								muscles[i].state.muscleDamperAdd = 0f;
								muscles[i].positionOffset = Vector3.zero;

								// Change physic materials
								SetColliders(muscles[i], false);
							}
						}
					}

					// Call events
					state = State.Puppet;

					if (eventsEnabled) {
						onRegainBalance.Trigger(puppetMaster);
						if (onRegainBalance.switchBehaviour) return;
					}

					break;

				// Switching to the UNPINNED state
				case State.Unpinned:
					unpinnedTimer = 0f;
					getUpTimer = 0f;
					getupAnimationBlendWeight = 0f;

					for (var i = 0; i < muscles.Length; i++) {
						if (hasBoosted) muscles[i].state.immunity = 0f;

						if (maxRigidbodyVelocity != Mathf.Infinity) {
							muscles[i].rigidbody.velocity = Vector3.ClampMagnitude(muscles[i].rigidbody.velocity, maxRigidbodyVelocity);
							muscles[i].mappedVelocity = Vector3.ClampMagnitude(muscles[i].mappedVelocity, maxRigidbodyVelocity);
						}

						puppetMaster.muscles[i] = muscles[i];

						// Change physic materials
						SetColliders(muscles[i], true);
					}

					// Drop all the props
					if (dropProps) {
						dropPropFlag = true;
					}

					for (var i = 0; i < muscles.Length; i++) {
						if (!muscles[i].state.isDisconnected) {
							muscles[i].state.muscleWeightMlp = puppetMaster.isAlive ? unpinnedMuscleWeightMlp : puppetMaster.stateSettings.deadMuscleWeight;
						}
					}

					// Trigger events
					onLoseBalance.Trigger(puppetMaster, puppetMaster.isAlive);
					if (onLoseBalance.switchBehaviour) {
						state = State.Unpinned;
						return;
					}

					// Trigger some more events
					if (state == State.Puppet) {
						onLoseBalanceFromPuppet.Trigger(puppetMaster, puppetMaster.isAlive);
						if (onLoseBalanceFromPuppet.switchBehaviour) {
							state = State.Unpinned;
							return;
						}
					} else {
						onLoseBalanceFromGetUp.Trigger(puppetMaster, puppetMaster.isAlive);
						if (onLoseBalanceFromGetUp.switchBehaviour) {
							state = State.Unpinned;
							return;
						}
					}

					// Unpin the muscles. This is done after the events in case behaviours are switched and the next behaviour might need the weights as they were
					for (var i = 0; i < muscles.Length; i++) {
						muscles[i].state.pinWeightMlp = 0f;
					}

					break;

				// Switching to the GETUP state
				case State.GetUp:
					unpinnedTimer = 0f;
					getUpTimer = 0f;
					hasCollidedSinceGetUp = false;

					// Is the ragdoll facing up or down?
					bool isProne = IsProne();
					state = State.GetUp;

					// Trigger events
					if (isProne) {
						onGetUpProne.Trigger(puppetMaster);
						if (onGetUpProne.switchBehaviour) return;
					} else {
						onGetUpSupine.Trigger(puppetMaster);
						if (onGetUpSupine.switchBehaviour) return;
					}

					// Unpin the puppet just in case
					foreach (Muscle m in puppetMaster.muscles) {
						if (!m.state.isDisconnected) {
							/*
							m.state.muscleWeightMlp = 0f;
							m.state.pinWeightMlp = 0f;
							m.state.muscleDamperAdd = 0f;
							*/

							// Change physic materials
							SetColliders(m, false);
						}
					}

					// Set the target's rotation
					Quaternion spineRot = puppetMaster.muscles[0].rigidbody.rotation;
					Vector3 spineDirection = puppetMaster.muscles[0].rigidbody.rotation * hipsUp;
					Vector3 normal = puppetMaster.targetRoot.up;
					Vector3.OrthoNormalize(ref normal, ref spineDirection);
					RotateMover(Quaternion.LookRotation((isProne ? spineDirection : -spineDirection), puppetMaster.targetRoot.up));
					puppetMaster.muscles[0].transform.rotation = spineRot;

					// RotateTarget(Quaternion.LookRotation((isProne ? spineDirection : -spineDirection), puppetMaster.targetRoot.up));

					// Set the target's position
					puppetMaster.SampleTargetMappedState();
					Vector3 getUpOffset = isProne ? getUpOffsetProne : getUpOffsetSupine;
					MoveTarget(puppetMaster.muscles[0].rigidbody.position + puppetMaster.targetRoot.rotation * getUpOffset);
					GroundTarget(groundLayers);
					getUpPosition = puppetMaster.targetRoot.position;

					getupAnimationBlendWeight = 1f;
					getUpTargetFixed = false;

					break;
			}

			// Apply the new puppet state
			state = newState;
		}


		/// <summary>
		/// Sets the colliders of the puppet to pinned/unpinned materials.
		/// </summary>
		public void SetColliders(bool unpinned) {
			foreach (Muscle m in puppetMaster.muscles) {
				SetColliders(m, unpinned);
			}
		}

		// Sets colliders of a muscle to puppet or unpinned mode 
		public void SetColliders(in Muscle m, bool unpinned) {
			var props = GetProps(m.props.group);

			if (unpinned) {
				foreach (Collider c in m.colliders) {
					c.material = props.unpinnedMaterial != null ? props.unpinnedMaterial : defaults.unpinnedMaterial;

					// Enable colliders
					if (props.disableColliders) c.enabled = true;
				}
			} else {
				foreach (Collider c in m.colliders) {
					c.material = props.puppetMaterial != null ? props.puppetMaterial : defaults.puppetMaterial;

					// Enable colliders
					if (props.disableColliders) c.enabled = false;
				}
			}
		}

		public override void OnMuscleDisconnected(Muscle m) {
			base.OnMuscleDisconnected(m);

			SetColliders(m, true);
		}

		public override void OnMuscleReconnected(Muscle m) {
			base.OnMuscleReconnected(m);

			if (m == puppetMaster.muscles[0]) {
				SetState(State.Puppet);
			}

			float f = state == State.Puppet ? 1f : 0f;
			m.state.pinWeightMlp = f;
			m.state.muscleWeightMlp = f;
			m.state.muscleDamperMlp = f;
			m.state.maxForceMlp = 1;
			m.state.mappingWeightMlp = 1f;

			SetColliders(m, state == State.Unpinned);
		}
	}
}