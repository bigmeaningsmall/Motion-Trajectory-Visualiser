using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothLook : MonoBehaviour{
    
    public int selectedCamera = 1;
    
    public Transform targetObject;
    public Transform predictedObject;
    public Transform cameraTarget;
    
    private Vector3 selectedPosition;
    public float smoothTime = 0.5f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    private void Awake(){
        targetObject = GameObject.FindWithTag("TargetObject").GetComponent<Transform>();
        predictedObject = GameObject.FindWithTag("PredictedObject").GetComponent<Transform>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            selectedCamera = 1;
        }   
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            selectedCamera = 2;
        }   
        if (Input.GetKeyDown(KeyCode.Alpha3)){
            selectedCamera = 3;
        }

        switch (selectedCamera){
            case 1:
                selectedPosition = TransformUtilities.GetPosition(targetObject, predictedObject) + offset;
                break;
            case 2:
                selectedPosition = targetObject.position + offset;
                break;
            case 3:
                selectedPosition = predictedObject.position + offset;
                break;
        }
    }

    void LateUpdate()
    {
        // Calculate the target position based on the target's position and the offset
        // selectedPosition = cameraTarget.position + offset;

        // Smoothly move the camera to the target position over time
        transform.position = Vector3.SmoothDamp(transform.position, selectedPosition, ref velocity, smoothTime);

        // Make the camera look at the target
        cameraTarget.position = selectedPosition;
        transform.LookAt(cameraTarget);
    }
}
