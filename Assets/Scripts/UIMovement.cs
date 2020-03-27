using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovement : MonoBehaviour
{
    public static UIMovement instance;

    Camera worldCamera;

    [SerializeField]
    RectTransform armyData, terrainData;
    
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
                armyData.position = new Vector3(0f, Screen.height, armyData.position.z);
            }
            else
            {
                armyData.position = new Vector3(Screen.width - armyData.rect.width, Screen.height, armyData.position.z);
            }
        }
        else
        {
            if (cursorPositionForCamera.x > Screen.width / 2)
            {
                terrainData.position = new Vector3(0f, 0f + terrainData.rect.height, terrainData.position.z);
            }
            else
            {
                terrainData.position = new Vector3(Screen.width - terrainData.rect.width, 0f + terrainData.rect.height, terrainData.position.z);
            }
        }
    }
}
