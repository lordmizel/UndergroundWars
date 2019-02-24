using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovement : MonoBehaviour
{
    public static UIMovement instance;

    Camera worldCamera;

    [SerializeField]
    RectTransform armyData, terrainData;

    [SerializeField]
    RectTransform upperLeft, upperRight, lowerRight, lowerLeft;
    bool down = false, right = false;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        worldCamera = CameraController.instance.gameObject.GetComponent<Camera>();
    }

    public void MoveAside(Vector3 cursorPosition)
    {
        Vector3 cursorPositionForCamera = worldCamera.WorldToScreenPoint(cursorPosition);

        if(cursorPositionForCamera.y > Screen.height / 2)
        {
            if (cursorPositionForCamera.x > Screen.width / 2)
            {
                armyData.position = upperLeft.position;
            }
            else
            {
                armyData.position = upperRight.position;
            }
        }
        else
        {
            if (cursorPositionForCamera.x > Screen.width / 2)
            {
                terrainData.position = lowerLeft.position;
            }
            else
            {
                terrainData.position = lowerRight.position;
            }
        }
    }
}
