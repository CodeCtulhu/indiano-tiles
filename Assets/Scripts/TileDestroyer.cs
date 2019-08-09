using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileDestroyer : MonoBehaviour
{
    public void DestroyTile()
    {
        TileController.createdTiles.Remove(gameObject);
        Destroy(gameObject);
    }
}
