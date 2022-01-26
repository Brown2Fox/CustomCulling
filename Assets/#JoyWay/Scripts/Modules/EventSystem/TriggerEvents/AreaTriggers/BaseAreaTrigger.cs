using System;
using UnityEngine;

public abstract class BaseAreaTrigger : MonoBehaviour {
	[Flags]
	public enum TriggerTypes {
		Player = 1 << 0,
		Bot = 1 << 1,
		Item = 1 << 2,
	}

	public TriggerTypes triggers = TriggerTypes.Player;

	protected Collider lastTriggerredObject;

	private void OnTriggerEnter(Collider other) {
		lastTriggerredObject = other;

		if ((triggers & TriggerTypes.Player) != 0)
			if (other.CompareTag("Player"))
				Trigger();

		if ((triggers & TriggerTypes.Bot) != 0)
			if (other.CompareTag("Bot"))
				Trigger();

		if ((triggers & TriggerTypes.Item) != 0)
			if (other.CompareTag("Item"))
				Trigger();
	}


	public abstract void Trigger();
}
