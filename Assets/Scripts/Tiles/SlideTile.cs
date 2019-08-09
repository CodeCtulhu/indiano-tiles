using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideTile : TileBase
{
    bool isSliding;

    public enum SlideState
    {
        IDLE,
        STARTED,
        SLIDING,
        ENDED
    }

    public SlideState slideState = SlideState.IDLE;

    public GameObject fillTile;
    // Update is called once per frame
    void Update()
    {
        MoveTile();
    }

    private void Awake()
    {
        fillTile = GetComponentInChildren<FillTile>().gameObject;
    }
}
