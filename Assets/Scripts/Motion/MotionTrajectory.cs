using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class MotionTrajectory : MonoBehaviour{

    public bool enabled = true;
    
    [Header("CONTROL")]
    public DataType motionType = DataType.Velocity;
    public Trajectory trajectoryType = Trajectory.Target;

    public Vector3 motionTrajectory;
    public Vector3 motionCoordinate;
    public Vector3 motionVelocity;
    public float scale;
    public Vector3 offset;
    public float velocityResetThreshold = 0.05f;

    [Header("CONTROL OBJECTS")] 
    public Transform controlObject;
    public Rigidbody rigidBody;

    private Vector3 initialPosition;
    public float velocityThreshold = 0.08f;
    public bool resetPosition = false;

    #region Events Subscriptions

    private void OnEnable(){
        UI_ManagerSettings.OnUpdateUI+= UI_ManagerOnUpdateUI;
        DataReader.OnTrajectoryEvent += DataReaderOnTrajectoryEvent;
        UI_ManagerControlPanel.OnResetEffectors += UI_ManagerControlPanelOnResetEffectors;
    }
    private void OnDisable(){
        UI_ManagerSettings.OnUpdateUI-= UI_ManagerOnUpdateUI;
        DataReader.OnTrajectoryEvent -= DataReaderOnTrajectoryEvent;
        UI_ManagerControlPanel.OnResetEffectors -= UI_ManagerControlPanelOnResetEffectors;
    }
    private void UI_ManagerOnUpdateUI(DataType dataType, float scale, Vector3 offsets){
        motionType = dataType;
        this.scale = scale;
        offset = offsets;
    }
    private void DataReaderOnTrajectoryEvent(Trajectory trajectoryType, Vector3 trajectory){
        //this.trajectoryType = trajectoryType;

        if (enabled){
            motionTrajectory = new Vector3(trajectory.x, trajectory.y, trajectory.z) + offset;
        
            // Debug.Log(trajectoryType);
            if (motionType == DataType.Coordinate){
                motionCoordinate = motionTrajectory;
            }
            if (motionType == DataType.Velocity){
                //todo apply to target and predicted
                if (this.trajectoryType == trajectoryType){
                    motionTrajectory = new Vector3(trajectory.x * scale, trajectory.y * scale, trajectory.z * scale);
                    motionVelocity = motionTrajectory;
                    // Debug.Log(motionVelocity.y);
                    rigidBody.velocity = motionVelocity;
                
                    // if (motionVelocity.y > -velocityThreshold && motionVelocity.y < velocityThreshold){
                    //     rigidBody.velocity = motionVelocity;
                    //     resetPosition = false;
                    // }
                    // else{
                    //     resetPosition = true;
                    // }
                }
            }
        }

    }
    private void UI_ManagerControlPanelOnResetEffectors(){
        this.transform.position = initialPosition;
    }

    #endregion
    void Start(){
        initialPosition = this.transform.position;
    }

    //TODO Write a reset end effector function here or in a new class...
    
    void Update(){
        // if (resetPosition){
        //     this.transform.position = Vector3.Lerp(this.transform.position, initialPosition, Time.deltaTime*2);
        // }
    }



}
