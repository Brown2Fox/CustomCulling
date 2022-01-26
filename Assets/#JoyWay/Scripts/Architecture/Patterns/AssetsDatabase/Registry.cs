using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JoyWay.Registries {
	public class RegistryBase : ScriptableObject {
		public bool autoFill;

		public virtual void PrepareRegistry() {
		}

#if UNITY_EDITOR
		[MenuItem("Registries/Prepare registries")]
		public static void PrepareRegistries() {
			Debug.Log("Preparing registries...");
			string[] guids = AssetDatabase.FindAssets("t:" + typeof(RegistryBase).Name);
			foreach (var guid in guids) {
				string path = AssetDatabase.GUIDToAssetPath(guid);
				RegistryBase registry = AssetDatabase.LoadAssetAtPath<RegistryBase>(path);
				registry.PrepareRegistry();

				EditorUtility.SetDirty(registry);
				AssetDatabase.SaveAssets();
			}

			Debug.Log("Done");
		}
#endif
	}

	public class Registry<T> : RegistryBase
		where T : RegistryEntryBase {
		public List<T> entries;

#if UNITY_EDITOR
		public virtual bool Check(T obj) {
			return obj;
		}

		public override void PrepareRegistry() {
			if (!autoFill) {
				Debug.Log($"{typeof(T)} autofill set to FALSE, skipping", this);
				return;
			}

			int id = 0;
			Debug.Log($"{typeof(T)}", this);

			entries?.Clear();
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
			foreach (string guid in guids) {
				string path = AssetDatabase.GUIDToAssetPath(guid);
				T definition = AssetDatabase.LoadAssetAtPath<T>(path);
				if (Check(definition)) {
					Debug.Log("   Adding definition:" + definition, definition);
					entries.Add(definition);
					definition.entryID = id++;

					EditorUtility.SetDirty(definition);
				}
			}

			EditorUtility.SetDirty(this);
		}
#endif
	}
}