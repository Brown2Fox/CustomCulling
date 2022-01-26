using System;
using System.Collections.Generic;
using UnityEngine;

public class EasyGameManager : Singleton<EasyGameManager> {
	public Action onBossSpawned;
	public Action<int> onBossPhase;
	public Action onBossDefeated;
	public Action<GameStates> onGameStateChange;

	public float levelTime;

	public void BossSpawned() {
		onBossSpawned?.Invoke();
	}

	public void BossPhase(int _phase) {
		onBossPhase?.Invoke(_phase);
	}


	public void BossDefeated() {
		onBossDefeated?.Invoke();
	}

	public void BeginDungeon() {
		levelTime = 0f;
	}

	private void Update() {
		levelTime += Time.deltaTime;
	}

	public enum GameStates {
		Intro,
		FakeHub1,
		FakeHub2,
		FakeDungeon,
		Tutorial,
		Dungeon,
		Hub
	}
}