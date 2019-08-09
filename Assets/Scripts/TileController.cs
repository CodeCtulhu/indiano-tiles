using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public static class TileMapUtilities
{
    public static TileBase GetTileMapDataFromMap(this TileBase tileBase, TileBaseContainer tileBaseContainer)
    {
        tileBase.tileYSize = tileBaseContainer.tileYSize;
        tileBase.tileType = (TileBase.TileType)tileBaseContainer.tileType;
        tileBase.yAxisEnd = tileBaseContainer.yAxisEnd;
        tileBase.yAxisOrigin = tileBaseContainer.yAxisOrigin;

        return tileBase;
    }
}

public class TileController : MonoBehaviour
{
    public static List<GameObject> createdTiles = new List<GameObject>();
    public static float tileSpeed = 0;

    public float currentSongPos;
    [SerializeField] public TileMapData tileMap;

    private AudioSource audioSource;
    private GameObject currentTile;
    private TileMapData.TileInfo currentTileInfo;
    private float distanceBeforeLine;

    //public static TileBase lastTile;
    private bool gameIsPlaying = false;

    private bool hasGameStarted = false;
    private bool isCurrentTileDequeued;

    [SerializeField] private Transform[] laneSpawnLocation = new Transform[4];
    [SerializeField] private Transform tapLanePosition;

    private float musicTrackLength;
    private int previousChosenTile;

    private GameObject tapTilePrefab, slideTilePrefab;
    private Queue<TileMapData.TileInfo> tileQueue = new Queue<TileMapData.TileInfo>();

    private float timeBeforeStart;
    private float timeTracked = 0;

    public enum GameState
    {
        NOT_STARTED,
        FIRST_CYCLE,
        SECOND_CYCLE,
        THIRD_CYCLE,
        INFINTE_MODE
    }

    public static GameState gameState = GameState.NOT_STARTED;
    private bool areTilesMoving;

    public void ChangeTileMode()
    {

        switch (gameState)
        {
            case GameState.NOT_STARTED:
                gameState = GameState.FIRST_CYCLE;
                break;
            case GameState.FIRST_CYCLE:
                gameState = GameState.SECOND_CYCLE;
                break;
            case GameState.SECOND_CYCLE:
                gameState = GameState.THIRD_CYCLE;
                break;
            case GameState.THIRD_CYCLE:
                gameState = GameState.INFINTE_MODE;
                break;
            case GameState.INFINTE_MODE:
                gameState = GameState.SECOND_CYCLE;
                break;
            default:
                break;
        }
    }

    void InitializeGame()
    {
        switch (gameState)
        {
            case GameState.FIRST_CYCLE:

                tileQueue = new Queue<TileMapData.TileInfo>(tileMap.tileSequence);

                tileSpeed = 2;

                Invoke("InitializeFunction",3);

                break;
            case GameState.SECOND_CYCLE:

                tileQueue = new Queue<TileMapData.TileInfo>(tileMap.tileSequence);

                tileSpeed = 4;
                Invoke("InitializeFunction", 3);


                break;
            case GameState.THIRD_CYCLE:

                tileQueue = new Queue<TileMapData.TileInfo>(tileMap.tileSequence);

                tileSpeed = 6;

                Invoke("InitializeFunction", 3);

                break;
            case GameState.INFINTE_MODE:

                //tileSpeed = 2;
                break;
            default:
                break;
        }

        
    }

    private void InitializeFunction()
    {
        areTilesMoving = true;
        audioSource.pitch = 1 * tileSpeed / 2;
        timeBeforeStart = (distanceBeforeLine / tileSpeed);
        Invoke("StartMusic", timeBeforeStart);
        InvokeRepeating("TimeTrack", 0, 0.01f);
    }

    void ResetGame()
    {
        CancelInvoke();
        areTilesMoving = false;
        timeTracked = 0;
    }

    public Vector3 GetRandomLane()
    {
        int randomNumber;
        do
        {
            randomNumber = UnityEngine.Random.Range(0, 4);
        } while (randomNumber == previousChosenTile);
        previousChosenTile = randomNumber;

        return laneSpawnLocation[randomNumber].position;
    }

    public void StartMusic()
    {
        audioSource.Play();
    }

    private void Awake()
    {
        tapTilePrefab = Resources.Load<GameObject>("Tap Tile");
        slideTilePrefab = Resources.Load<GameObject>("Slide Tile");

        audioSource = GetComponent<AudioSource>();

        audioSource.clip = tileMap.musicTrack;

        distanceBeforeLine = laneSpawnLocation[0].position.y - tapLanePosition.position.y;

        musicTrackLength = audioSource.clip.length;
    }

    private void Start()
    {
        audioSource.mute = true ;
        gameIsPlaying = true;
    }

    private void TimeTrack()
    {
        timeTracked += 0.01f * tileSpeed;
    }

    public static void GameLost()
    {
        tileSpeed = 0;
    }

    private void Update()
    {
        //tiles start to generate some time before music starts playing and finish generating sometime before music
        if (gameIsPlaying)
        {
            if (!isCurrentTileDequeued)
            {
                if (tileQueue.Count == 0 && createdTiles.Count == 0)
                {
                    ResetGame();
                    ChangeTileMode();
                    InitializeGame();
                }
                else
                {
                    currentTileInfo = tileQueue.Dequeue();
                    isCurrentTileDequeued = true;
                }
            }

            if (currentTileInfo.tileTimePosStart <= timeTracked && areTilesMoving)
            {
                if (currentTileInfo.tile.tileType == TileBaseContainer.TileType.TAP_TILE)
                {
                    currentTile = Instantiate(tapTilePrefab, GetRandomLane(), Quaternion.identity);
                    createdTiles.Add(currentTile);
                }
                else if (currentTileInfo.tile.tileType == TileBaseContainer.TileType.SLIDE_TILE)
                {
                    currentTile = Instantiate(slideTilePrefab, GetRandomLane(), Quaternion.identity);
                    createdTiles.Add(currentTile);
                }

                TileBase tileBase = currentTile.GetComponent<TileBase>();

                tileBase.GetTileMapDataFromMap(currentTileInfo.tile);
                //lastTile = tileBase;

                if (currentTileInfo.tile.tileType == TileBaseContainer.TileType.SLIDE_TILE)
                {
                    currentTile.transform.localScale = new Vector3
                    {
                        x = currentTile.transform.localScale.x,
                        y = tileBase.tileYSize / 10,
                        z = 1
                    };
                }

                currentTile.transform.position = new Vector3    
                {
                    x = currentTile.transform.position.x,
                    y = currentTile.transform.position.y + tileBase.tileYSize / 2,
                    z = -2
                };

                isCurrentTileDequeued = false;
            }
        }
    }
}