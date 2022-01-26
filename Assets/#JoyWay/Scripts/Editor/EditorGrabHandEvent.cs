using UnityEditor;

[CustomEditor(typeof(GrabHandEvent))]
public class EditorGrabHandEvent : Editor
{
    GrabHandEvent grabHandEvent;

    private void OnEnable() {
        grabHandEvent = (GrabHandEvent)target;
    }
    public override void OnInspectorGUI() {
        EditorGUILayout.BeginVertical();

        grabHandEvent.handType = (HandType)EditorGUILayout.EnumPopup("Type", grabHandEvent.handType);

        if(grabHandEvent.handType == HandType.Left) {
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onLeftGrab"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onLeftUnGrab"), true);
        }
        if (grabHandEvent.handType == HandType.Right) {
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onRightGrab"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onRightUnGrab"), true);
        }
        if (grabHandEvent.handType == HandType.Any) {
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onLeftGrab"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onLeftUnGrab"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onRightGrab"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onRightUnGrab"), true);
        }
        this.serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
    }
}
