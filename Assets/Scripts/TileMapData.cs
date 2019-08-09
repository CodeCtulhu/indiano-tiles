using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileMapData", menuName = "Game Scriptable Object",order = 0)]
public class TileMapData : ScriptableObject
{
    // how the fuck do i make this thing contain a sequence for a song?
    /* ok so we need to store the tiles, there are 2 types of tiles 
     * one has the same size and it's a tap
     * the other is a hold slide tile which will have varying tiles
     * those 2 types of tiles will be arranged in an order that the game will then place randomly 
     * by checking if the lane is the same or not.
     * 
     */


    [SerializeField] public List<TileInfo> tileSequence = new List<TileInfo>();
    public AudioClip musicTrack;

    [Serializable]
    public struct TileInfo
    {
        [SerializeField] public TileBaseContainer tile;
        public float tileTimePosStart;
    }
        /*
         * So how do i arrange those tiles?
         * What do i need?
         * 1. Their "position" in the song.
         * 2. The whole songs duration so that i can start and end it
         * 3. The tiles need to be synchronized with the map
         * 4. The Map contains the song as well since they are related
         * 5. The map needs to have a boolean that determines if the song is in infinite mode or not
         * 6. The map is only a container to that data, and Game Controller is what will scroll the map down
         */ 

}
