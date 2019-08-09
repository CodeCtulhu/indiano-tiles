using System;
using UnityEditor;
using UnityEngine;

public class TileMapConstructorWindow : EditorWindow
{
    public static TileMapConstructorWindow window;

    private GameObject tapTilePrefab;
    private GameObject slideTilePrefab;
    private GameObject lastTile;
    private float lastYPos;
    private float tileLaneOrigin;
    private float tileCreationSize;

    #region DrawCreatorUI()

    private Vector3 startingLineOrigin = new Vector3(2, 0, 0);
    private Vector3 startingLineTo = new Vector3(-2, 0, 0);

    #endregion DrawCreatorUI()

    #region GUI variables

    private Texture2D headerSectionTexture;
    private Texture2D contentsSectionTexture;

    private Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    private Rect headerSection;
    private Rect contentsSection;

    private GUISkin skin;

    private static GUIStyle ToggleButtonStyleNormal = null;
    private static GUIStyle ToggleButtonStyleToggled = null;

    private AudioSource audioSource;

    private bool isPaused;

    [NonSerialized] public bool isSlideTile = false;
    [NonSerialized] public bool isTapTile = false;
    [NonSerialized] private bool isDoubleTapTile = false;
    [NonSerialized] private bool isDoubleSlideTile = false;

    public enum switchTileEnum
    {
        SLIDE_TILE,
        TAP_TILE,
        DOUBLE_TAP_TILE,
        DOUBLE_SLIDE_TILE
    }

    public static switchTileEnum switchTile;

    #endregion GUI variables

    #region Scriptable Objects

    private TileMapData tileMapData;
    private string tilemapDataName;

    public SerializedObject SerializedData;
    public SerializedProperty _tileSequenceArray;

    private bool isTileCreationOn;
    private bool isSequenceWindowOpen;

    private readonly string TILE_MAPS_DATA_PATH = "Assets/TileMaps/";

    #endregion Scriptable Objects

    [MenuItem("Window/Tile Map Creator")]
    private static void OpenWindow()
    {
        window = (TileMapConstructorWindow)GetWindow<TileMapConstructorWindow>();

        window.minSize = new UnityEngine.Vector2(300, 300);
        //window.maxSize = new UnityEngine.Vector2(600, 300);

        window.Show();
    }

    private void SceneGUI(SceneView sceneView)
    {
        // This will have scene events including mouse down on scenes objects
        Event guiEvent = Event.current;

        if (isTileCreationOn)
        {
            DrawCreatorUI();

            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

            float drawPlaneHeight = 0;
            float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.z) / mouseRay.direction.z;
            Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
            {
                Debug.Log(switchTile);
                AddTileToSequence(mousePosition.y);
            }

            if (guiEvent.keyCode == KeyCode.K)
            {
                window.Close();
                isTileCreationOn = false;
            }
        }

        void AddTileToSequence(float mouseYPos)
        {
            Vector3 placePos = new Vector3
            {
                x = 0.5f,
                y = lastYPos,
                z = 0
            };

            //Tile info to fill in to the list
            TileMapData.TileInfo tileInfo = new TileMapData.TileInfo();
            TileBaseContainer tile;
            float tileYOrigin;

            Debug.Log(isDoubleTapTile);

            float slideTileSize;
            GameObject instantiatedTile;

            Debug.Log(switchTile);

            switch (switchTile)
            {
                case switchTileEnum.SLIDE_TILE:
                    //calculate the size
                    slideTileSize = (float)(Math.Round(Convert.ToDouble(mouseYPos), 2)) - lastYPos;
                    tileYOrigin = lastYPos;
                    lastYPos += slideTileSize;
                    placePos.y += slideTileSize / 2;

                    Debug.Log(slideTilePrefab);

                    instantiatedTile = Instantiate(slideTilePrefab, placePos, Quaternion.identity);
                    lastTile = instantiatedTile;

                    instantiatedTile.transform.localScale = new Vector3
                    {
                        x = instantiatedTile.transform.localScale.x,
                        y = slideTileSize / 10,
                        z = 1
                    };

                    tile = new TileBaseContainer();

                    tile.tileType = TileBaseContainer.TileType.SLIDE_TILE;
                    tile.yAxisOrigin = tileYOrigin;
                    tile.yAxisEnd = lastYPos;
                    tile.tileYSize = slideTileSize;

                    tileInfo.tile = tile;
                    //I probably need to do some calculations here idfk how
                    tileInfo.tileTimePosStart = tileYOrigin - tileLaneOrigin;
                    break;

                case switchTileEnum.TAP_TILE:
                    tileYOrigin = lastYPos;
                    lastYPos += TapTile.tileYSize;
                    placePos.y += TapTile.tileYSize / 2;

                    lastTile = Instantiate(tapTilePrefab, placePos, Quaternion.identity);

                    tile = new TileBaseContainer();
                    tile.tileType = TileBaseContainer.TileType.TAP_TILE;
                    tile.yAxisOrigin = tileYOrigin;
                    tile.yAxisEnd = lastYPos;
                    tile.tileYSize = TapTile.tileYSize;

                    tileInfo.tile = tile;
                    //I probably need to do some calculations here idfk howd
                    tileInfo.tileTimePosStart = tileYOrigin - tileLaneOrigin;
                    break;

                case switchTileEnum.DOUBLE_TAP_TILE:
                    tileYOrigin = lastYPos;
                    lastYPos += TapTile.tileYSize;
                    placePos.y += TapTile.tileYSize / 2;

                    lastTile = Instantiate(tapTilePrefab, placePos, Quaternion.identity);

                    tile = new TileBaseContainer();
                    tile.tileType = TileBaseContainer.TileType.TAP_TILE;
                    tile.yAxisOrigin = tileYOrigin;
                    tile.yAxisEnd = lastYPos;
                    tile.tileYSize = TapTile.tileYSize;

                    tileInfo.tile = tile;
                    //I probably need to do some calculations here idfk howd
                    tileInfo.tileTimePosStart = tileYOrigin - tileLaneOrigin;
                    break;

                case switchTileEnum.DOUBLE_SLIDE_TILE:
                    Debug.Log("Does this shit actually work?");
                    //calculate the size
                    slideTileSize = (float)(Math.Round(Convert.ToDouble(mouseYPos), 2)) - lastYPos;
                    tileYOrigin = lastYPos;
                    lastYPos += slideTileSize;
                    placePos.y += slideTileSize / 2;

                    Debug.Log(slideTilePrefab);

                    instantiatedTile = Instantiate(slideTilePrefab, placePos, Quaternion.identity);
                    lastTile = instantiatedTile;

                    instantiatedTile.transform.localScale = new Vector3
                    {
                        x = instantiatedTile.transform.localScale.x,
                        y = slideTileSize / 10,
                        z = 1
                    };

                    tile = new TileBaseContainer();

                    tile.tileType = TileBaseContainer.TileType.SLIDE_TILE;
                    tile.yAxisOrigin = tileYOrigin;
                    tile.yAxisEnd = lastYPos;
                    tile.tileYSize = slideTileSize;

                    tileInfo.tile = tile;
                    //I probably need to do some calculations here idfk how
                    tileInfo.tileTimePosStart = tileYOrigin - tileLaneOrigin;
                    break;

                default:
                    break;
            }

            tileMapData.tileSequence.Add(tileInfo);
        }

        void DrawCreatorUI()
        {
            Debug.DrawLine(startingLineOrigin, startingLineTo, Color.red);
        }
    }

    private void OnEnable()
    {
        float lineY = FindObjectOfType<Lane>().transform.position.y;
        startingLineOrigin.y = lineY;
        startingLineTo.y = lineY;

        tileLaneOrigin = lineY;

        lastYPos = lineY;

        skin = Resources.Load<GUISkin>("Editor Skin");

        tapTilePrefab = Resources.Load<GameObject>("Tap Tile");
        slideTilePrefab = Resources.Load<GameObject>("Slide Tile");

        audioSource = Camera.main.GetComponent<AudioSource>();

        SceneView.duringSceneGui += SceneGUI;

        InitTextures();
        InitData();
    }

    private void OnGUI()
    {
        if (ToggleButtonStyleNormal == null)
        {
            ToggleButtonStyleNormal = "Button";
            ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
            ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
        }

        ScriptableObject target = tileMapData;
        SerializedData = new SerializedObject(target);
        _tileSequenceArray = SerializedData.FindProperty("tileSequence");

        DrawLayouts();
        DrawHeader();
        DrawContents();
    }

    private void InitData()
    {
        tileMapData = ScriptableObject.CreateInstance<TileMapData>();
    }

    private void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        contentsSectionTexture = new Texture2D(1, 1);
        contentsSectionTexture.SetPixel(0, 0, new Color(1, 1, 1));
        contentsSectionTexture.Apply();
    }

    private void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 70;

        contentsSection.x = 0;
        contentsSection.y = headerSection.height;
        contentsSection.width = Screen.width;
        contentsSection.height = Screen.height - headerSection.height;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(contentsSection, contentsSectionTexture);
    }

    private void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUILayout.BeginVertical();
        GUILayout.Label("Tile Map Creator", skin.GetStyle("Header1"));
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void DrawContents()
    {
        GUILayout.BeginArea(contentsSection);

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Tile map name", GUILayout.ExpandWidth(false));
        tileMapData.name = EditorGUILayout.TextField(tileMapData.name);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Music Track", GUILayout.ExpandWidth(false));
        tileMapData.musicTrack = (AudioClip)EditorGUILayout.ObjectField(tileMapData.musicTrack, typeof(AudioClip), false);
        audioSource.clip = tileMapData.musicTrack;
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create Tile Map"))
        {
            AssetDatabase.CreateAsset(tileMapData, TILE_MAPS_DATA_PATH + tileMapData.name + ".asset");
        }

        if (audioSource.clip != null)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("play song"))
            {
                audioSource.Play();
            }

            if (isPaused)
            {
                if (GUILayout.Button("Unpause song"))
                {
                    audioSource.UnPause();
                    isPaused = false;
                }
            }
            else
            {
                if (GUILayout.Button("Pause song"))
                {
                    audioSource.Pause();
                    isPaused = true;
                }
            }

            if (GUILayout.Button("Stop song"))
            {
                audioSource.Stop();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(GetTrackTime(audioSource.time));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("00:00", GUILayout.ExpandWidth(false));

            audioSource.time = GUILayout.HorizontalSlider(audioSource.time, 0f, audioSource.clip.length);

            GUILayout.Label(GetTrackTime(audioSource.clip.length), GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();

        isTileCreationOn = GUILayout.Toggle(isTileCreationOn, "Turn on Scene Creator");

        if (isTileCreationOn)
        {
            if (GUILayout.Button("Tap Tile", (switchTile == switchTileEnum.TAP_TILE) ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
            {
                switchTile = switchTileEnum.TAP_TILE;
            }
            if (GUILayout.Button("Slide Tile", (switchTile == switchTileEnum.SLIDE_TILE) ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
            {
                switchTile = switchTileEnum.SLIDE_TILE;
            }

            if (GUILayout.Button("Double Tap Tile", (switchTile == switchTileEnum.DOUBLE_TAP_TILE) ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
            {
                switchTile = switchTileEnum.DOUBLE_TAP_TILE;
            }

            if (GUILayout.Button("Double Slide Tile", (switchTile == switchTileEnum.DOUBLE_SLIDE_TILE) ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
            {
                switchTile = switchTileEnum.DOUBLE_SLIDE_TILE;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Remove Last Tile"))
            {
                Destroy(lastTile);
            }

            GUILayout.BeginHorizontal();

            EditorGUILayout.FloatField(tileCreationSize);

            GUILayout.EndHorizontal();
        }

        if (isTileCreationOn)
        {
            if (GUILayout.Button("Open Tile Sequence Window")) ;
            {
                TileSequenceWindow.OpenWindow();
            }
        }

        GUILayout.EndArea();
    }

    private static string GetTrackTime(float time)
    {
        int trackMusicLength = Convert.ToInt32(Mathf.Floor(time / 60));
        string stringTime = trackMusicLength + ":" + Convert.ToString(Mathf.Round(time - trackMusicLength * 60));
        return stringTime;
    }

    public class TileSequenceWindow : EditorWindow
    {
        private Texture2D contentsSectionTexture;

        private Rect contentsSection;

        private GUISkin skin;

        public static TileSequenceWindow window;

        public static void OpenWindow()
        {
            window = (TileSequenceWindow)GetWindow<TileSequenceWindow>();

            window.minSize = new Vector2(300, 300);
            window.maxSize = new Vector2(400, 900);

            window.Show();
        }

        private void OnEnable()
        {
            InitTextures();
        }

        private void OnGUI()
        {
            DrawLayouts();
            DrawContents();
        }

        private void InitTextures()
        {
            contentsSectionTexture = new Texture2D(1, 1);
            contentsSectionTexture.SetPixel(0, 0, new Color(1, 1, 1));
            contentsSectionTexture.Apply();
        }

        private void DrawLayouts()
        {
            contentsSection.x = 0;
            contentsSection.y = 0;
            contentsSection.width = Screen.width;
            contentsSection.height = Screen.height;

            GUI.DrawTexture(contentsSection, contentsSectionTexture);
        }

        private void DrawContents()
        {
            GUILayout.BeginArea(contentsSection);

            EditorGUILayout.PropertyField(TileMapConstructorWindow.window._tileSequenceArray, true);
            TileMapConstructorWindow.window.SerializedData.ApplyModifiedProperties();

            GUILayout.EndArea();
        }

        private static string GetTrackTime(float time)
        {
            int trackMusicLength = Convert.ToInt32(Mathf.Floor(time / 60));
            string stringTime = trackMusicLength + ":" + Convert.ToString(Mathf.Round(time - trackMusicLength * 60));
            return stringTime;
        }
    }
}