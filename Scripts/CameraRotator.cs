using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private Transform point;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(point.position,Vector3.up, rotateSpeed* Time.deltaTime);
    }
}
