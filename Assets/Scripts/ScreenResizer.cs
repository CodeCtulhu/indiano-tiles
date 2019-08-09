using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResizer : MonoBehaviour
{

    public SpriteRenderer gameplaySize;
    // Start is called before the first frame update
    void Start()
    {
        float orthoSize = gameplaySize.bounds.size.x * Screen.height / Screen.width * 0.5f;

        Camera.main.orthographicSize = orthoSize;

        gameplaySize.gameObject.transform.position = new Vector3
        {
            x = gameplaySize.gameObject.transform.position.x,
            y = gameplaySize.gameObject.transform.position.y + gameplaySize.bounds.size.y / 2 - (Camera.main.orthographicSize - Camera.main.transform.position.y),
            z = gameplaySize.gameObject.transform.position.z
        };

        //float screenRatio = (float)Screen.width / (float)Screen.height;
        //float targetRatio = gameplaySize.bounds.size.x / gameplaySize.bounds.size.y;

        //if (screenRatio >= targetRatio)
        //{
        //    Camera.main.orthographicSize = gameplaySize.bounds.size.y / 2;
        //}
        //else
        //{
        //    float differenceRatio = targetRatio / screenRatio;
        //    Camera.main.orthographicSize = gameplaySize.bounds.size.y / 2 * differenceRatio;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
