using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfiguratableJointExtention {
	public static ConfiguratableJointsFields MakeCopy(this ConfigurableJoint _joint) {
		ConfiguratableJointsFields copy = new ConfiguratableJointsFields() {
			connectedBody = _joint.connectedBody,
			axis = _joint.axis,
			anchor = _joint.anchor,
			connectedAnchor = _joint.connectedAnchor,
			autoConfigureConnectedAnchor = _joint.autoConfigureConnectedAnchor,
			breakForce = _joint.breakForce,
			breakTorque = _joint.breakTorque,
			enableCollision = _joint.enableCollision,
			enablePreprocessing = _joint.enablePreprocessing,
			massScale = _joint.massScale,
			connectedMassScale = _joint.connectedMassScale,

			projectionAngle = _joint.projectionAngle,
			projectionDistance = _joint.projectionDistance,
			projectionMode = _joint.projectionMode,
			slerpDrive = _joint.slerpDrive,
			angularYZDrive = _joint.angularYZDrive,
			angularXDrive = _joint.angularXDrive,
			rotationDriveMode = _joint.rotationDriveMode,
			targetAngularVelocity = _joint.targetAngularVelocity,
			targetRotation = _joint.targetRotation,
			zDrive = _joint.zDrive,
			yDrive = _joint.yDrive,
			xDrive = _joint.xDrive,
			targetVelocity = _joint.targetVelocity,
			targetPosition = _joint.targetPosition,
			angularZLimit = _joint.angularZLimit,
			angularYLimit = _joint.angularYLimit,
			highAngularXLimit = _joint.highAngularXLimit,
			lowAngularXLimit = _joint.lowAngularXLimit,
			linearLimit = _joint.linearLimit,
			angularYZLimitSpring = _joint.angularYZLimitSpring,
			angularXLimitSpring = _joint.angularXLimitSpring,
			linearLimitSpring = _joint.linearLimitSpring,
			angularZMotion = _joint.angularZMotion,
			angularYMotion = _joint.angularYMotion,
			angularXMotion = _joint.angularXMotion,
			zMotion = _joint.zMotion,
			yMotion = _joint.yMotion,
			xMotion = _joint.xMotion,
			secondaryAxis = _joint.secondaryAxis,
			configuredInWorldSpace = _joint.configuredInWorldSpace,
			swapBodies = _joint.swapBodies
		};
		return copy;
	}

	public static ConfigurableJoint Copy(this ConfigurableJoint _joint, ConfiguratableJointsFields _source) {
		_joint.connectedBody = _source.connectedBody;
		_joint.axis = _source.axis;
		_joint.anchor = _source.anchor;
		_joint.connectedAnchor = _source.connectedAnchor;
		_joint.autoConfigureConnectedAnchor = _source.autoConfigureConnectedAnchor;
		_joint.breakForce = _source.breakForce;
		_joint.breakTorque = _source.breakTorque;
		_joint.enableCollision = _source.enableCollision;
		_joint.enablePreprocessing = _source.enablePreprocessing;
		_joint.massScale = _source.massScale;
		_joint.connectedMassScale = _source.connectedMassScale;

		_joint.projectionAngle = _source.projectionAngle;
		_joint.projectionDistance = _source.projectionDistance;
		_joint.projectionMode = _source.projectionMode;
		_joint.slerpDrive = _source.slerpDrive;
		_joint.angularYZDrive = _source.angularYZDrive;
		_joint.angularXDrive = _source.angularXDrive;
		_joint.rotationDriveMode = _source.rotationDriveMode;
		_joint.targetAngularVelocity = _source.targetAngularVelocity;
		_joint.targetRotation = _source.targetRotation;
		_joint.zDrive = _source.zDrive;
		_joint.yDrive = _source.yDrive;
		_joint.xDrive = _source.xDrive;
		_joint.targetVelocity = _source.targetVelocity;
		_joint.targetPosition = _source.targetPosition;
		_joint.angularZLimit = _source.angularZLimit;
		_joint.angularYLimit = _source.angularYLimit;
		_joint.highAngularXLimit = _source.highAngularXLimit;
		_joint.lowAngularXLimit = _source.lowAngularXLimit;
		_joint.linearLimit = _source.linearLimit;
		_joint.angularYZLimitSpring = _source.angularYZLimitSpring;
		_joint.angularXLimitSpring = _source.angularXLimitSpring;
		_joint.linearLimitSpring = _source.linearLimitSpring;
		_joint.angularZMotion = _source.angularZMotion;
		_joint.angularYMotion = _source.angularYMotion;
		_joint.angularXMotion = _source.angularXMotion;
		_joint.zMotion = _source.zMotion;
		_joint.yMotion = _source.yMotion;
		_joint.xMotion = _source.xMotion;
		_joint.secondaryAxis = _source.secondaryAxis;
		_joint.configuredInWorldSpace = _source.configuredInWorldSpace;
		_joint.swapBodies = _source.swapBodies;

		return _joint;
	}
}

public struct ConfiguratableJointsFields {      //
												// Сводка:
												//     A reference to another rigidbody this joint connects to.
	public Rigidbody connectedBody;
	//
	// Сводка:
	//     The Direction of the axis around which the body is constrained.
	public Vector3 axis;
	//
	// Сводка:
	//     The Position of the anchor around which the joints motion is constrained.
	public Vector3 anchor;
	//
	// Сводка:
	//     Position of the anchor relative to the connected Rigidbody.
	public Vector3 connectedAnchor;
	//
	// Сводка:
	//     Should the connectedAnchor be calculated automatically?
	public bool autoConfigureConnectedAnchor;
	//
	// Сводка:
	//     The force that needs to be applied for this joint to break.
	public float breakForce;
	//
	// Сводка:
	//     The torque that needs to be applied for this joint to break. To be able to break,
	//     a joint must be _Locked_ or _Limited_ on the axis of rotation where the torque
	//     is being applied. This means that some joints cannot break, such as an unconstrained
	//     Configurable Joint.
	public float breakTorque;
	//
	// Сводка:
	//     Enable collision between bodies connected with the joint.
	public bool enableCollision;
	//
	// Сводка:
	//     Toggle preprocessing for this joint.
	public bool enablePreprocessing;
	//
	// Сводка:
	//     The scale to apply to the inverse mass and inertia tensor of the body prior to
	//     solving the constraints.
	public float massScale;
	//
	// Сводка:
	//     The scale to apply to the inverse mass and inertia tensor of the connected body
	//     prior to solving the constraints.
	public float connectedMassScale;

	//
	// Сводка:
	//     Set the angular tolerance threshold (in degrees) for projection. If the joint
	//     deviates by more than this angle around its locked angular degrees of freedom,
	//     the solver will move the bodies to close the angle. Setting a very small tolerance
	//     may result in simulation jitter or other artifacts. Sometimes it is not possible
	//     to project (for example when the joints form a cycle).
	public float projectionAngle;
	//
	// Сводка:
	//     Set the linear tolerance threshold for projection. If the joint separates by
	//     more than this distance along its locked degrees of freedom, the solver will
	//     move the bodies to close the distance. Setting a very small tolerance may result
	//     in simulation jitter or other artifacts. Sometimes it is not possible to project
	//     (for example when the joints form a cycle).
	public float projectionDistance;
	//
	// Сводка:
	//     Brings violated constraints back into alignment even when the solver fails. Projection
	//     is not a physical process and does not preserve momentum or respect collision
	//     geometry. It is best avoided if practical, but can be useful in improving simulation
	//     quality where joint separation results in unacceptable artifacts.
	public JointProjectionMode projectionMode;
	//
	// Сводка:
	//     Definition of how the joint's rotation will behave around all local axes. Only
	//     used if Rotation Drive Mode is Slerp Only.
	public JointDrive slerpDrive;
	//
	// Сводка:
	//     Definition of how the joint's rotation will behave around its local Y and Z axes.
	//     Only used if Rotation Drive Mode is Swing & Twist.
	public JointDrive angularYZDrive;
	//
	// Сводка:
	//     Definition of how the joint's rotation will behave around its local X axis. Only
	//     used if Rotation Drive Mode is Swing & Twist.
	public JointDrive angularXDrive;
	//
	// Сводка:
	//     Control the object's rotation with either X & YZ or Slerp Drive by itself.
	public RotationDriveMode rotationDriveMode;
	//
	// Сводка:
	//     This is a Vector3. It defines the desired angular velocity that the joint should
	//     rotate into.
	public Vector3 targetAngularVelocity;
	//
	// Сводка:
	//     This is a Quaternion. It defines the desired rotation that the joint should rotate
	//     into.
	public Quaternion targetRotation;
	//
	// Сводка:
	//     Definition of how the joint's movement will behave along its local Z axis.
	public JointDrive zDrive;
	//
	// Сводка:
	//     Definition of how the joint's movement will behave along its local Y axis.
	public JointDrive yDrive;
	//
	// Сводка:
	//     Definition of how the joint's movement will behave along its local X axis.
	public JointDrive xDrive;
	//
	// Сводка:
	//     The desired velocity that the joint should move along.
	public Vector3 targetVelocity;
	//
	// Сводка:
	//     The desired position that the joint should move into.
	public Vector3 targetPosition;
	//
	// Сводка:
	//     Boundary defining rotation restriction, based on delta from original rotation.
	public SoftJointLimit angularZLimit;
	//
	// Сводка:
	//     Boundary defining rotation restriction, based on delta from original rotation.
	public SoftJointLimit angularYLimit;
	//
	// Сводка:
	//     Boundary defining upper rotation restriction, based on delta from original rotation.
	public SoftJointLimit highAngularXLimit;
	//
	// Сводка:
	//     Boundary defining lower rotation restriction, based on delta from original rotation.
	public SoftJointLimit lowAngularXLimit;
	//
	// Сводка:
	//     Boundary defining movement restriction, based on distance from the joint's origin.
	public SoftJointLimit linearLimit;
	//
	// Сводка:
	//     The configuration of the spring attached to the angular Y and angular Z limits
	//     of the joint.
	public SoftJointLimitSpring angularYZLimitSpring;
	//
	// Сводка:
	//     The configuration of the spring attached to the angular X limit of the joint.
	public SoftJointLimitSpring angularXLimitSpring;
	//
	// Сводка:
	//     The configuration of the spring attached to the linear limit of the joint.
	public SoftJointLimitSpring linearLimitSpring;
	//
	// Сводка:
	//     Allow rotation around the Z axis to be Free, completely Locked, or Limited according
	//     to Angular ZLimit.
	public ConfigurableJointMotion angularZMotion;
	//
	// Сводка:
	//     Allow rotation around the Y axis to be Free, completely Locked, or Limited according
	//     to Angular YLimit.
	public ConfigurableJointMotion angularYMotion;
	//
	// Сводка:
	//     Allow rotation around the X axis to be Free, completely Locked, or Limited according
	//     to Low and High Angular XLimit.
	public ConfigurableJointMotion angularXMotion;
	//
	// Сводка:
	//     Allow movement along the Z axis to be Free, completely Locked, or Limited according
	//     to Linear Limit.
	public ConfigurableJointMotion zMotion;
	//
	// Сводка:
	//     Allow movement along the Y axis to be Free, completely Locked, or Limited according
	//     to Linear Limit.
	public ConfigurableJointMotion yMotion;
	//
	// Сводка:
	//     Allow movement along the X axis to be Free, completely Locked, or Limited according
	//     to Linear Limit.
	public ConfigurableJointMotion xMotion;
	//
	// Сводка:
	//     The joint's secondary axis.
	public Vector3 secondaryAxis;
	//
	// Сводка:
	//     If enabled, all Target values will be calculated in world space instead of the
	//     object's local space.
	public bool configuredInWorldSpace;
	//
	// Сводка:
	//     If enabled, the two connected rigidbodies will be swapped, as if the joint was
	//     attached to the other body.
	public bool swapBodies;
}