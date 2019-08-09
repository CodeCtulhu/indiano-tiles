using UnityEditor;


[CustomEditor(typeof(TileController))]
public class TileControllerEditor : Editor
{
    private SerializedProperty _tileMap;


    private void OnEnable()
    {
        _tileMap = serializedObject.FindProperty("tileMap");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Tile Map contents",EditorStyles.boldLabel);
        
        CreateEditor(_tileMap.objectReferenceValue).OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}
