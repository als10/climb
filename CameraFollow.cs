using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 startPos;
    
    public Transform target;

    public float speed = 10f;

    public float distanceFromCentre = 1.92f;

    void Start ()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (target.position.y + distanceFromCentre > transform.position.y) 
        {
            Vector3 targetPos = target.position;
            targetPos.x = transform.position.x;
            targetPos.y += distanceFromCentre;
            targetPos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    public void reset() {
        transform.position = startPos;
    }
}
