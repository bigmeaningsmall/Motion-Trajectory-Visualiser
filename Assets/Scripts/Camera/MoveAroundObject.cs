using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundObject : MonoBehaviour{
    
    [SerializeField] private float mouseSensitivity = 2.0f;

    [SerializeField] private Transform target;
    
    [Header("ROTATION")]
    private float rotationY;
    private float rotationX;
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private Vector2 rotationXMinMax = new Vector2(-40, 40);
    
    [Header("ROTATION")]
    [SerializeField] private float distanceFromTarget = 1.0f;
    public float minDistance = 0.2f;
    public float maxDistance = 4f;
    private float currentDistance;
    private float desiredDistance;

    private void Start(){
        currentDistance = distanceFromTarget;
        desiredDistance = distanceFromTarget;
    }

    void Update(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * mouseSensitivity * 4;
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, smoothTime * Time.deltaTime);
        distanceFromTarget = currentDistance;

        rotationY += mouseX;
        rotationX += mouseY;

        // Apply clamping for x rotation 
        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        // Apply damping between rotation changes
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;


        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = target.position - transform.forward * distanceFromTarget;

        if (Input.GetMouseButtonDown(0)){
            
        }
    }
}