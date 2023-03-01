using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using ChartAndGraph;
using Random = UnityEngine.Random;

public class GraphFeed : MonoBehaviour{
    
    [Header("Attach to Graph to Supply Realtime Data")]
    private GraphChart graph;

    [Header("Signal Type")] 
    public GraphDisplay axis = GraphDisplay.XYZ;
    public bool showTarget = true;
    public bool showPredicted = true;

    public Vector3 targetVector;
    public float targetDisplay;
    public Vector3 predictedVector;
    public float predictedDisplay;

    public bool graphEnabled = true;
    public bool simulateGraph = false;
    private float multiplier = 1f;
    private float interval = 0.05f;
    private float timer;
    private float hoz = 10;

    #region Event Subscriptions

    private void OnEnable(){
        // UI_ManagerSettings.OnUpdateUI+= UI_ManagerOnUpdateUI;
        DataReader.OnTrajectoryEvent += DataReaderOnTrajectoryEvent;
    }
    private void OnDisable(){
        // UI_ManagerSettings.OnUpdateUI-= UI_ManagerOnUpdateUI;
        DataReader.OnTrajectoryEvent -= DataReaderOnTrajectoryEvent;
    }

    private void DataReaderOnTrajectoryEvent(Trajectory trajectoryType, Vector3 trajectory){
        if (!simulateGraph){
            if (trajectoryType == Trajectory.Target){
                targetVector = trajectory;
                targetDisplay = SetAxisValue(targetVector);
            }
            if (trajectoryType == Trajectory.Predicted){
                predictedVector = trajectory;
                predictedDisplay = SetAxisValue(predictedVector);
            }
        }
    }
    #endregion
    
    private void Awake(){
        graph = gameObject.GetComponent<GraphChart>();
        if (graph != null){

            graph.Scrollable = true;
            //graph.HorizontalValueToStringMap[0.0] = "Zero"; // example of how to set custom axis strings
            graph.DataSource.StartBatch();
            graph.DataSource.ClearCategory("target");
            graph.DataSource.ClearCategory("predicted");
            graph.DataSource.EndBatch();
        }
    }

    void Start(){

    }


    void Update () {
        timer += Time.deltaTime;
        if (timer >= interval){
            if (simulateGraph){
                targetVector = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            }

            timer = 0;
				
            if (graphEnabled)
            {
                // graph.DataSource.AddPointToCategoryRealtime("target", System.DateTime.Now, targetVector.y, interval);
                if (showTarget){
                    graph.DataSource.AddPointToCategoryRealtime("target", hoz, targetDisplay, interval);  
                }
                if (showPredicted){
                    graph.DataSource.AddPointToCategoryRealtime("predicted", hoz, predictedDisplay, interval);
                }
                
            }

            //hoz = hoz + interval;
            // graph.HorizontalValueToStringMap[0.0] = Time.time.ToString();
            //hoz = Time.time;
            // graph.VerticalValueToStringMap[0.0] = Time.time.ToString("f2");
            graph.HorizontalValueToStringMap[System.DateTime.Now.Second] = System.DateTime.Now.Second.ToString();
        }
        hoz += Time.deltaTime;
        // hoz = System.DateTime.Now;
    }

    private float SetAxisValue(Vector3 trajectory){
        float t = 0;

        switch (axis)
        {
            case GraphDisplay.X:
                float x = trajectory.x;
                t = x;
                break;
            case GraphDisplay.Y:
                float y = trajectory.y;
                t = y;
                break;
            case GraphDisplay.Z:
                float z = trajectory.z;
                t = z;
                break;
            case GraphDisplay.XY:
                float xy = (trajectory.x+trajectory.y)/2;
                t = xy;
                break;
            case GraphDisplay.YZ:
                float yz = (trajectory.y+trajectory.z)/2;
                t = yz;
                break;
            case GraphDisplay.XZ:
                float xz = (trajectory.x+trajectory.z)/2;
                t = xz;
                break;
            case GraphDisplay.XYZ:
                float xyz = (trajectory.x+trajectory.y+trajectory.z)/3;
                t = xyz;
                break;
        }

        t = t * multiplier;
        return t;
    }

    IEnumerator ClearAll(float delay){
        yield return new WaitForSeconds(delay);
        GraphChartBase graph = GetComponent<GraphChartBase>();

        graph.DataSource.Clear();
    }
}
