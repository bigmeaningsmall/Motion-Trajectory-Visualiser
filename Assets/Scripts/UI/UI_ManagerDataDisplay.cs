using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enums;

public class UI_ManagerDataDisplay : MonoBehaviour{
    
    public DataType motionType = DataType.Velocity;
    public Trajectory trajectoryType;

    public Vector3 targetVelocity;
    public Vector3 predictedVelocity;
    
    [Header("DATA DISPLAY UI")]
    public Slider xTargetVelocity;
    public Slider yTargetVelocity;
    public Slider zTargetVelocity;
    public TextMeshProUGUI xTargetVelocitySliderText;
    public TextMeshProUGUI yTargetVelocitySliderText;
    public TextMeshProUGUI zTargetVelocitySliderText;
    public Slider xPredictedVelocity;
    public Slider yPredictedVelocity;
    public Slider zPredictedVelocity;
    public TextMeshProUGUI xPredictedVelocitySliderText;
    public TextMeshProUGUI yPredictedVelocitySliderText;
    public TextMeshProUGUI zPredictedVelocitySliderText;
    
    #region Events Subscriptions

    private void OnEnable(){
        UI_ManagerSettings.OnUpdateUI+= UI_ManagerOnUpdateUI;
        DataReader.OnTrajectoryEvent += DataReaderOnTrajectoryEvent;
    }
    private void OnDisable(){
        UI_ManagerSettings.OnUpdateUI-= UI_ManagerOnUpdateUI;
        DataReader.OnTrajectoryEvent -= DataReaderOnTrajectoryEvent;
    }
    private void UI_ManagerOnUpdateUI(DataType dataType, float scale, Vector3 offsets){
        motionType = dataType;
    }
    private void DataReaderOnTrajectoryEvent(Trajectory trajectoryType, Vector3 trajectory){
        this.trajectoryType = trajectoryType;
        if (motionType == DataType.Coordinate){
            
        }
        if (motionType == DataType.Velocity){
            if (trajectoryType == Trajectory.Target){
                targetVelocity = trajectory;
            }
            if (trajectoryType == Trajectory.Predicted){
                predictedVelocity = trajectory;
            }
            UpdateVelocityXYZ();
        }
    }
    
    #endregion
    
    void Start()
    {
        // xTargetVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // yTargetVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // zTargetVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // xPredictedVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // yPredictedVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // zPredictedVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
    }

    //temp bool
    private bool menuActive = true;
    private void Update(){
        //TEMP CONTROLS
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            if (menuActive){
                // Get all child objects of this game object
                foreach (Transform child in transform){
                    // Deactivate each child object
                    child.gameObject.SetActive(false);
                }
                GetComponent<Image>().enabled = false;
                menuActive = false;
            }
            else{
                // Get all child objects of this game object
                foreach (Transform child in transform){
                    // Deactivate each child object
                    child.gameObject.SetActive(true);
                }
                GetComponent<Image>().enabled = true;
                menuActive = true;
            }
        }
    }

    public void UpdateVelocityXYZ(){
        xTargetVelocity.value = targetVelocity.x;
        yTargetVelocity.value = targetVelocity.y;
        zTargetVelocity.value = targetVelocity.z;
        xPredictedVelocity.value = predictedVelocity.x;
        yPredictedVelocity.value = predictedVelocity.y;
        zPredictedVelocity.value = predictedVelocity.z;
        xTargetVelocitySliderText.text = xTargetVelocity.value.ToString("f2");
        yTargetVelocitySliderText.text = yTargetVelocity.value.ToString("f2");
        zTargetVelocitySliderText.text = zTargetVelocity.value.ToString("f2");
        xPredictedVelocitySliderText.text = xPredictedVelocity.value.ToString("f2");
        yPredictedVelocitySliderText.text = yPredictedVelocity.value.ToString("f2");
        zPredictedVelocitySliderText.text = zPredictedVelocity.value.ToString("f2");
    }
}
