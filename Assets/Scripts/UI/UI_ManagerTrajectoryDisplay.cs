using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enums;

public class UI_ManagerTrajectoryDisplay : MonoBehaviour{
    
    public DataType motionType = DataType.Velocity;
    public Trajectory trajectoryType;
    
    Trajectory trajectoryDisplay;
    Axis axis = Axis.XYZ;

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

    [Header("Graph Buttons")] 
    private bool graphTargetActive = false;
    private bool graphPredictedActive = false;
    public Image buttonTarget; 
    public Image buttonPredicted; 
    public Image buttonX; 
    public Image buttonY; 
    public Image buttonZ;
    public Image buttonXYZ;
    public Image buttonXY; 
    public Image buttonYZ; 
    public Image buttonXZ;
    
    
    #region Events

    public delegate void UpdateTrajectoryDisplay( bool t, Trajectory trajectoryDisplay);
    public static event UpdateTrajectoryDisplay OnUpdateTrajectoryDisplay;
    public delegate void UpdateTrajectoryAxis( Axis axis);
    public static event UpdateTrajectoryAxis OnUpdateTrajectoryAxis;

    #endregion

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
        
        ButtonGraphDisplay(1);
        ButtonGraphDisplay(2);
        ButtonGraphAxis(4);
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

    #region Graph Buttons

    public void ButtonGraphDisplay(int index){
        if (index == 1){
            if (graphTargetActive){
                graphTargetActive = false;
                buttonTarget.color = StaticData.instance.UI_Default;
                if (OnUpdateTrajectoryDisplay != null){
                    OnUpdateTrajectoryDisplay(false, Trajectory.Target);
                }
            }
            else{
                graphTargetActive = true;
                buttonTarget.color = StaticData.instance.UI_SelectedTarget;  
                if (OnUpdateTrajectoryDisplay != null){
                    OnUpdateTrajectoryDisplay(true, Trajectory.Target);
                }
            }
        }

        if (index == 2){
            if (graphPredictedActive){
                graphPredictedActive = false;
                buttonPredicted.color = StaticData.instance.UI_Default;
                if (OnUpdateTrajectoryDisplay != null){
                    OnUpdateTrajectoryDisplay(false, Trajectory.Predicted);
                }
            }
            else{
                graphPredictedActive = true;
                buttonPredicted.color = StaticData.instance.UI_SelectedPredicted;  
                if (OnUpdateTrajectoryDisplay != null){
                    OnUpdateTrajectoryDisplay(true, Trajectory.Predicted);
                }
            }
        }
    }
    public void ButtonGraphAxis(int index){
        ResetAxisButtons();
        
        switch (index){
            case 1:
                buttonX.color = StaticData.instance.UI_Selected;
                axis = Axis.X;
                break;
            case 2:
                buttonY.color = StaticData.instance.UI_Selected;
                axis = Axis.Y;
                break;
            case 3:
                buttonZ.color = StaticData.instance.UI_Selected;
                axis = Axis.Z;
                break;
            case 4:
                buttonXYZ.color = StaticData.instance.UI_Selected;
                axis = Axis.XYZ;
                break;
            case 5:
                buttonXY.color = StaticData.instance.UI_Selected;
                axis = Axis.XY;
                break;
            case 6:
                buttonYZ.color = StaticData.instance.UI_Selected;
                axis = Axis.YZ;
                break;
            case 7:
                buttonXZ.color = StaticData.instance.UI_Selected;
                axis = Axis.XZ;
                break;
        }
        if (OnUpdateTrajectoryAxis != null){
            OnUpdateTrajectoryAxis(axis);
        }
    }
    private void ResetAxisButtons(){
        buttonX.color = StaticData.instance.UI_Default;
        buttonY.color = StaticData.instance.UI_Default;
        buttonZ.color = StaticData.instance.UI_Default;
        buttonXYZ.color = StaticData.instance.UI_Default;
        buttonXY.color = StaticData.instance.UI_Default;
        buttonYZ.color = StaticData.instance.UI_Default;
        buttonXZ.color = StaticData.instance.UI_Default;
    }

    #endregion

}
