using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Light weight state machine
/// </summary>
/// <typeparam name="Enum"></typeparam>
[Serializable]
public class StateMachine {
	public event Action<Enum, Enum> OnStateSwitchOut, OnStateSwitchIn;
	public Dictionary<Enum, int> mask;

	public delegate bool AdditionalCheckAction(Enum from, Enum to);

	public AdditionalCheckAction AdditionalCheck;

	public StateMachine(Type T) {
		EnumEqualityComparer eeq = new EnumEqualityComparer();
		m_States = new Dictionary<Enum, State>(eeq);

		Array enums = Enum.GetValues(T);
		for (int i = enums.Length - 1; i >= 0; --i)
			Add((Enum)enums.GetValue(i));

		m_CurrentState = m_States[(Enum)enums.GetValue(0)];
	}

	public void Add(Enum id) {
		m_States.Add(id, new State(id));
	}

	public Enum CurrentState() {
		return m_CurrentState.Id;
	}

	public void Update() {
		m_CurrentState?.InvokeUpdate();
	}

	public void Shutdown() {
		m_CurrentState?.InvokeEnd();
		m_CurrentState = null;
	}


	public void AddStartListener(Enum id, Action action) {
		State state = m_States[id];
		state.Start += action;
	}

	public void AddUpdateListener(Enum id, Action action) {
		State state = m_States[id];
		state.Update += action;
	}

	public void AddEndListener(Enum id, Action action) {
		State state = m_States[id];
		state.End += action;
	}

	public void RemoveStartListener(Enum id, Action action) {
		State state = m_States[id];
		state.Start -= action;
	}

	public void RemoveUpdateListener(Enum id, Action action) {
		State state = m_States[id];
		state.Update -= action;
	}

	public void RemoveEndListener(Enum id, Action action) {
		State state = m_States[id];
		state.End -= action;
	}

	public bool Check(Enum state) {
		if (!m_States.ContainsKey(state)) {
			return false;
		}

		if (AdditionalCheck != null) {
			if (!AdditionalCheck.Invoke(m_CurrentState.Id, state))
				return false;
		}

		if (mask == null) return true;
		if (!mask.TryGetValue(m_CurrentState.Id, out int m)) return true;

		int s = (int)(object)state;
		if ((m & s) == s) return true;


		return false;
	}

	public bool SwitchTo(Enum state) {
		if (!Check(state)) return false;

		State newState = m_States[state];

		m_CurrentState?.InvokeEnd();

		OnStateSwitchOut?.Invoke(m_CurrentState.Id, newState.Id);
		OnStateSwitchIn?.Invoke(m_CurrentState.Id, newState.Id);

		m_CurrentState = newState;
		newState?.InvokeStart();

		return true;
	}

	[Serializable]
	class State {
		public State(Enum id) {
			Id = id;
		}

		public Enum Id;
		public event Action Start;
		public event Action Update;
		public event Action End;


		public void InvokeStart() {
			Start?.Invoke();
		}

		public void InvokeUpdate() {
			Update?.Invoke();
		}

		public void InvokeEnd() {
			End?.Invoke();
		}
	}

	State m_CurrentState = null;
	Dictionary<Enum, State> m_States;
}

public class EnumEqualityComparer : IEqualityComparer<Enum> {
	public bool Equals(Enum x, Enum y) {
		return x.GetHashCode() == y.GetHashCode();
	}

	public int GetHashCode(Enum obj) {
		return ((int)(object)obj).GetHashCode();
	}
}
