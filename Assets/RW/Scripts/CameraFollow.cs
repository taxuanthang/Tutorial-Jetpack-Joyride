using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private float distanceToTarget;

    public GameObject targetObject;

    public void Start()
    {
        distanceToTarget = transform.position.x - targetObject.transform.position.x;
    }

    public void Update()
    {
        float targetObjectX = targetObject.transform.position.x;
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectX + distanceToTarget;
        transform.position = newCameraPosition;
    }
}
