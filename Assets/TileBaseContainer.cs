using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TileBaseContainer
{
    public float yAxisOrigin;
    public float yAxisEnd;

    public float tileYSize;

    public enum TileType
    {
        TAP_TILE,
        SLIDE_TILE,
        DOUBLE_SLIDE_TILE,
        DOUBLE_TAP_TILE
    }

    public TileType tileType;
}
