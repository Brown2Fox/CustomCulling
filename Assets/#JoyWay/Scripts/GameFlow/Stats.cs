using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public struct Stats {
	public BossStats	boss;
	public EnemiesStats enemy;
	public ItemsStat	items;

}


[Serializable]
public struct BossStats {
	public int spawned;
	public int phase_2;
	public int kills;
}

[Serializable]
public struct EnemiesStats {
	public int totalEnemiesKilled;
	[SerializeField]
	public SerializedDictionary<string, EnemyStat> enemies;


	public void EnemySeen(string _name) {
		EnemyStat stat;

		if (enemies.TryGetValue(_name, out stat)) {
			stat.Seen();
			enemies[_name] = stat;
		}
		else {
			stat = new EnemyStat();
			stat.Seen();
			enemies.Add(_name, stat);
		}
	}

	public void EnemyKilled(string _name) {
		EnemyStat stat;
		if (enemies.TryGetValue(_name, out stat)) {
			stat.Picked();
			enemies[_name] = stat;
		}
		else {
			stat = new EnemyStat();
			stat.Picked();
			enemies.Add(_name, stat);

			totalEnemiesKilled ++;
		}
	}

}

[Serializable]
public struct EnemyStat {
	public int kill;
	public int seen;


	public void Seen() {
		seen++;
	}

	public void Picked() {
		kill++;
	}
}



[Serializable]
public struct ItemsStat {
	public int pedestals;
	[SerializeField]
	public SerializedDictionary<string, ItemStat> entries;


	public void ItemSeen(string _name) {
		ItemStat stat;

		if (entries.TryGetValue(_name, out stat)) {
			stat.Seen();
			entries[_name] = stat;
		} else {
			stat = new ItemStat();
			stat.Seen();
			entries.Add(_name, stat);
		}
	}

	public void ItemPicked(string _name) {
		ItemStat stat;

		if (entries.TryGetValue(_name, out stat)) {
			stat.Picked();
			entries[_name] = stat;
		} else {
			stat = new ItemStat();
			stat.Picked();
			entries.Add(_name, stat);
		}


		pedestals++;
	}
}

[Serializable]
public struct ItemStat {
	public int picked;
	public int seen;


	public void Seen() {
		seen++;
	}

	public void Picked() {
		picked++;
	}
}

[Serializable]
public struct StatField { }