using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    Vector2 lowerLeftLimit;
    Vector2 upperRightLimit;

    float halfHeight;
    float halfWidth;

    float placementCorrection = 0.51f;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Vector2 correctionVector = new Vector2(placementCorrection, placementCorrection);

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        lowerLeftLimit = new Vector2(0f, 0f) + new Vector2(halfWidth, halfHeight) - correctionVector;
        upperRightLimit = new Vector2(Map.instance.mapWidth, Map.instance.mapHeight) - new Vector2(halfWidth, halfHeight) - correctionVector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCameraToPoint(int x, int y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, lowerLeftLimit.x, upperRightLimit.x), Mathf.Clamp(transform.position.y, lowerLeftLimit.y, upperRightLimit.y), transform.position.z);
    }

}
