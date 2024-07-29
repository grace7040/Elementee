using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_HorizontalMovement : MonoBehaviour
{
    public Transform pivotPoint; // Point to pivot around
    public float horizontalSpeed = 1.0f; // Speed control variable for horizontal movement
    public float horizontalRange = 1.0f; // Range control variable for horizontal movement
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        // Calculate horizontal movement
        float horizontalMovementValue = Mathf.Sin(Time.time * horizontalSpeed) * horizontalRange * 0.01f;

        // Set the new position based on the pivot point
        Vector3 offsetFromPivot = transform.position - pivotPoint.position;
        Vector3 newPosition = pivotPoint.position + offsetFromPivot + Vector3.right * horizontalMovementValue;

        // Update the object's position
        transform.position = newPosition;
    }


}
