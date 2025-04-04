using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allignArrow : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed = 10f; // Speed of rotation
    private CharacterController characterController;
    private Vector3 lastPosition;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate movement direction based on position change
        Vector3 movementDirection = (transform.position - lastPosition).normalized;

        // Update the last position
        lastPosition = transform.position;

        // Rotate only if there is meaningful movement
        if (movementDirection.sqrMagnitude > 0.01f)
        {
            // Smoothly rotate towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

