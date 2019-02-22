using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovement : MonoBehaviour
{
    public static UIMovement instance;

    Camera worldCamera;

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

        if(cursorPositionForCamera.x > Screen.width / 2)
        {
            if (cursorPositionForCamera.y > Screen.height / 2)
            {
                transform.position = lowerLeft.position;
            }
            else
            {
                transform.position = upperLeft.position;
            }
        }
        else
        {
            if (cursorPositionForCamera.y > 0.5f)
            {
                transform.position = lowerRight.position;
            }
            else
            {
                transform.position = upperRight.position;
            }
        }
    }
}
