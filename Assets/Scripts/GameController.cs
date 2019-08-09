using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    Camera camera;
    bool isGameLost;


    SlideTile script;



    
    /*
    Use Tile objects with length and type description to put into a queue or a list to understand the way of placements 
    */
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) && !isGameLost)
        {
            Vector3 pressPos = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(pressPos);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Normal Tile"))
                {
                    if (hitInfo.collider.gameObject.GetComponent<TapTile>().isTileActive)
                    {
                        hitInfo.collider.gameObject.GetComponent<TapTile>().isTileActive = false;
                        hitInfo.collider.GetComponent<Animator>().SetTrigger("Pressed");
                    }
                }
                else if(hitInfo.collider.CompareTag("Slide Tile"))
                {   
                    script = hitInfo.collider.gameObject.GetComponent<SlideTile>();

                    if (script.slideState != SlideTile.SlideState.ENDED)
                    {
                        script.slideState = SlideTile.SlideState.STARTED;

                        script.fillTile.transform.localScale = new Vector3
                        {
                            x = script.fillTile.transform.localScale.x,
                            y = Mathf.Clamp((hitInfo.point.y - (script.transform.position.y - script.tileYSize / 2)) ,0, script.tileYSize) / script.tileYSize,
                            z = script.fillTile.transform.localScale.z
                        };
                    }

                }
                else if (hitInfo.collider.CompareTag("Background"))
                {
                    if (script.slideState != SlideTile.SlideState.SLIDING)
                    {
                        Debug.Log(TileController.createdTiles[0].GetComponent<TileBase>().yAxisOrigin);
                        TileController.GameLost();
                        //create red tile

                        isGameLost = true;
                    }
                }
            }
        }
        if (Input.touchCount > 0 && Input.touches[0].phase != TouchPhase.Ended && !isGameLost || Input.GetMouseButton(0))
        {
            Vector3 pressPos = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(pressPos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (script != null && script.slideState != SlideTile.SlideState.ENDED)
                {
                    script.slideState = SlideTile.SlideState.SLIDING;

                    script.fillTile.transform.localScale = new Vector3
                    {
                        x = script.fillTile.transform.localScale.x,
                        y = Mathf.Clamp((hitInfo.point.y - (script.transform.position.y - script.tileYSize / 2)), 0, script.tileYSize) / script.tileYSize,
                        z = script.fillTile.transform.localScale.z
                    };
                }
            }
        }
        else if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && !isGameLost || Input.GetMouseButtonUp(0))
        {
            if (script != null)
            {
                script.slideState = SlideTile.SlideState.ENDED;
                script.isTileActive = false;

                Vector3 pressPos = Input.mousePosition;
                Ray ray = camera.ScreenPointToRay(pressPos);
                RaycastHit hitInfo;

                Physics.Raycast(ray, out hitInfo);

                if ((hitInfo.point.y - (script.transform.position.y - script.tileYSize / 2)) > script.tileYSize)
                {
                    script.GetComponent<Animator>().SetTrigger("Pressed");
                }
            }
            Debug.Log("Mouse up");
        }
    }
}
