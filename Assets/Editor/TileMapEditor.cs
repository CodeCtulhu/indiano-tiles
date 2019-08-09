using UnityEditor;

[CustomEditor(typeof(TileMapData))]
public class TileMapEditor : Editor
{
    private SerializedProperty _tileSequence;
    private SerializedProperty _musicTrack;

    int listSize;

    private void OnEnable()
    {
        _tileSequence = serializedObject.FindProperty("tileSequence");
        _musicTrack = serializedObject.FindProperty("musicTrack");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_tileSequence,true);
        EditorGUILayout.PropertyField(_musicTrack);

        serializedObject.ApplyModifiedProperties();
    }
}