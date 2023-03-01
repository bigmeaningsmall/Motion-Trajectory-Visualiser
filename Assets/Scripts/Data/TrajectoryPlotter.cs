using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class TrajectoryPlotter : MonoBehaviour{
    
    #region Variables

    public Trajectory trajectoryType = Trajectory.Target;
    public Vector3 currentVector;
    
    
    #endregion
    
    #region Events Subscriptions

    private void OnEnable(){
        DataReader.OnTrajectoryEvent += DataReaderOnTrajectoryEvent;
    }
    private void OnDisable(){
        DataReader.OnTrajectoryEvent -= DataReaderOnTrajectoryEvent;
    }

    private void DataReaderOnTrajectoryEvent(Trajectory trajectoryType, Vector3 trajectory){
        if (this.trajectoryType == Trajectory.Target && trajectoryType == Trajectory.Target){
            currentVector = trajectory;
            // Debug.Log(trajectoryType.ToString() + ": " +  currentVector);
            
        }
        if (this.trajectoryType == Trajectory.Predicted && trajectoryType == Trajectory.Predicted){
            currentVector = trajectory;
            // Debug.Log(trajectoryType.ToString() + ": " +  currentVector);
        }

        transform.position = new Vector3(currentVector.x, currentVector.y, currentVector.z);
    }

    #endregion

    void Start(){
        
    }

    void Update(){
        
    }
}