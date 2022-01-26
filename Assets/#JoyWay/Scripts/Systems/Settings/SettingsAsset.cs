using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "SettingsAsset", menuName = "JoyWay/Settings", order = 0)]
public class SettingsAsset : ScriptableObject {
	[SerializeField]
	public SerializableSettings serializable;
	public int introChk => serializable.introCHK;


	[Header("other info")]
	public string path;
	public string saveName = "save.save";

	private void OnEnable() {
		path = Application.persistentDataPath + $"/settings/{saveName}";
	}


	public bool FileCheck() {
		if (string.IsNullOrEmpty(path)) OnEnable();

		bool b = File.Exists(path);
		if (!b) {
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}

			File.Create(path).Close();
		}

		return b;
	}


	public void CreateNewSave() {
		SetDefaultSave();

		string repath = path.Replace(saveName, $"broken_save_{DateTime.Now:yy_MM_dd_hhmmss}.th");

		File.Move(path, repath);
	}

	public void SetDefaultSave() {
		serializable = default;
		serializable.version = "1.0.1";
		serializable.input.turnType = TurnType.SnapTurn;
		serializable.input.snapTurnDegrees = SnapTurnDegrees._45;
		serializable.input.mainHand = HandType.Right;
		serializable.calibration.height = 178;
	}


	public void LoadFromFile() {
		if (!FileCheck()) {
			SetDefaultSave();
		} else {
			string json;
			using (StreamReader reader = new StreamReader(path)) {
				json = reader.ReadToEnd();
			}

			try {
				serializable = JsonUtility.FromJson<SerializableSettings>(json);
			}
			catch (Exception e) {
				Console.WriteLine(e);
				CreateNewSave();
			}

			ApplyFileToAsset();
		}
	}


	public void ApplyFileToAsset() {
		if (serializable.calibration.height <= 0)
			serializable.calibration.height = 178;
		else
			serializable.calibration.height = Mathf.Clamp(serializable.calibration.height, 90, 220);

	}


	public void ApplyAssetToFile() {
#if UNITY_EDITOR
#endif

		//serializable.stats.items.ItemPicked("boboa");

		SaveToFile();
	}


	public void SaveToFile() {
		FileCheck();

		using (StreamWriter writer = new StreamWriter(path)) {
			var json = JsonUtility.ToJson(serializable, true);
			writer.Write(json);
			writer.Flush();
		}
	}


	public void SaveProgress() {
		ApplyAssetToFile();
	}


	public HandType mainHand {
		get => serializable.input.mainHand;
		set => serializable.input.mainHand = value;
	}

	// 1 to 2 , 2 to 1
	public HandType offHand => (HandType) ((int) mainHand % 2 + 1);

	public void ResetTutorials() {
	}

	public void ResetStats() {
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(SettingsAsset))]
public class SettingsAssetEditor : Editor {
	Editor audioEditor;

	public override void OnInspectorGUI() {
		var me = target as SettingsAsset;

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("LoadFromFile")) {
			me.LoadFromFile();
		}

		if (GUILayout.Button("Open settings Folder")) {
			EditorUtility.RevealInFinder(me.path);
		}

		if (GUILayout.Button("SaveToFile")) {
			me.ApplyAssetToFile();
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		base.OnInspectorGUI();

		EditorGUILayout.Space();


		if (GUILayout.Button("ClearAll")) {
			me.SetDefaultSave();
		}

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("ClearTutorials")) {
			me.ResetTutorials();
		}

		if (GUILayout.Button("ClearStats")) {
			me.ResetStats();
		}

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();


		if (GUILayout.Button("SaveAsset")) {
			AssetDatabase.Refresh();
			EditorUtility.SetDirty(target);
			AssetDatabase.SaveAssets();
		}


		EditorGUILayout.Space();


		// //////////        AUDIO
		//
		// EditorGUILayout.BeginVertical();
		//
		// me.audio.volume = EditorGUILayout.Slider("Main", me.audio.volume, -80f, 20f);
		// EditorGUI.indentLevel++;
		// me.audio.ambientVolume = EditorGUILayout.Slider("Ambient", me.audio.ambientVolume, -80f, 20f);
		// me.audio.effectsVolume = EditorGUILayout.Slider("Effects", me.audio.effectsVolume, -80f, 20f);
		// me.audio.speechVolume = EditorGUILayout.Slider("Speech", me.audio.speechVolume, -80f, 20f);
		//
		// EditorGUI.indentLevel--;
		// EditorGUILayout.EndVertical();
		//
		//
		// if (EditorApplication.isPlaying){
		// 	AudioController.instance.Set(me.audio);
		// }
	}
}


#endif