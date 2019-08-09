using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    public float yAxisOrigin;
    public float yAxisEnd;

    public float tileYSize;

    public bool isTileActive = true;

    public enum TileType
    {
        TAP_TILE,
        SLIDE_TILE,
        DOUBLE_SLIDE_TILE,
        DOUBLE_TAP_TILE
    }

    public TileType tileType;

    protected void MoveTile()
    {
        transform.Translate(Vector3.down * TileController.tileSpeed * Time.deltaTime);
    }

    public void DisableTile()
    {
        isTileActive = false;
    }

    private void OnBecameInvisible()
    {
        if (isTileActive)
        {
            TileController.GameLost();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
