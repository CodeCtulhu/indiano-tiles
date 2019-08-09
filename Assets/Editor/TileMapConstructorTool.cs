using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMapConstructorWindow))]
public class TileMapConstructorTool : Editor
{
    /*
     * This tool will be used to create maps,
     * There will 2 buttons to select either a normal tile or a slide tile
     * The tool has to be able to create a tileMap with an array of TileInfo structs,
     * Each time a tile is created, it's tileTimePos must be recorded from the provided song.
     * (probably from the song itself)
     * normal tiles have fixed duration.
     * slide tiles are calculated from the next tile
     * The Creator needs to have a mini player inside
     */

    // Serialize this value to set a default value in the Inspector.
    [SerializeField]
    Texture2D m_ToolIcon;

    GUIContent m_IconContent;

    TileMapConstructorWindow tileMapConstructorWindow;

    void OnEnable()
    {
        tileMapConstructorWindow = target as TileMapConstructorWindow;
    }

    // This is called for each window that your tool is active in. Put the functionality of your tool here.
    void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck();

        Event guiEvent = Event.current;

        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

        float drawPlaneHeight = 0;
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.z) / mouseRay.direction.z;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            //Debug.Log(mousePosition);
        }

        //Vector3 position = Tools.handlePosition;
        //using (new Handles.DrawingScope(Color.green))
        //{
        //    position = Handles.Slider(position, Vector3.right);
        //}

        //if (EditorGUI.EndChangeCheck())
        //{
        //    Vector3 delta = position - Tools.handlePosition;

        //    Undo.RecordObjects(Selection.transforms, "Move Platform");

        //    foreach (var transform in Selection.transforms)
        //        transform.position += delta;
        //}
    }
}
