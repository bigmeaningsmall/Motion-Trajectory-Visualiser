using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float xSpeed = 120f;
    public float ySpeed = 120f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float smoothTime = 0.3f;

    private float x = 0f;
    private float y = 0f;
    private float currentDistance;
    private float desiredDistance;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        currentDistance = distance;
        desiredDistance = distance;
    }

    void LateUpdate()
    {
        // Calculate the target position based on the target's position and the distance
        Vector3 targetPosition = target.position - transform.forward * currentDistance;

        // Smoothly move the camera to the target position over time
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Get the mouse input and scroll wheel input
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * 1f;

        // Clamp the vertical angle to stay within the specified limits
        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        // Clamp the desired distance to stay within the specified limits
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        // Smoothly adjust the current distance to the desired distance over time
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, smoothTime * Time.deltaTime);

        // Set the rotation of the camera based on the mouse input and the clamped angle
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}
