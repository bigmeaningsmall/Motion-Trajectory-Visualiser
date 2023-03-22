using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class DataReader : MonoBehaviour{

    #region Variables

    public Trajectory trajectoryType = Trajectory.Target;
    
    // List for holding data from CSV reader
    List<Dictionary<string, object>> pointList;
    
    public Vector3[] vectorData = new Vector3[0];
    
    // Folder of the data from 'Resources'
    public string folderName;
    // Name of the input file, no extension
    public string inputFile;

    // Indices for columns to be assigned
    public int column1 = 0;
    public int column2 = 1;
    public int column3 = 2;
    
    // Full column names from CSV (as Dictionary Keys)
    public string xColumnName;
    public string yColumnName;
    public string zColumnName;
    
    //********Private Variables********
    // Minimum and maximum values of columns
    private float xMin;
    private float yMin;
    private float zMin;

    private float xMax;
    private float yMax;
    private float zMax;
    
    // Number of rows
    public int rowCount;

    //TODO use this to check when reading - when realtime is added needs to set a enum statemachine
    public bool streamingData = false;
    //todo - need to check data loaded before class can stream
    public bool dataLoaded = false;
    
    private Coroutine broadcastData;

    #endregion
    
    #region Events Subscriptions

    private void OnEnable(){
        FileBrowserCSV.OnFileBrowserEvent += FileBrowserCSVOnFileBrowserEvent;
    }
    private void OnDisable(){
        FileBrowserCSV.OnFileBrowserEvent -= FileBrowserCSVOnFileBrowserEvent;
    }
    private void FileBrowserCSVOnFileBrowserEvent(BrowserEvent eventType, string filePath, string fileName, string filePathPersist){

        if (trajectoryType == Trajectory.Target && eventType == BrowserEvent.LoadedTarget){
            // Debug.Log(trajectoryType);
            // Debug.Log(eventType);
            // Debug.Log(filePathPersist);
            folderName = filePathPersist;
            folderName = Application.persistentDataPath;
            inputFile = fileName;
            //inputFile = FileUtilities.RemoveFileExtension(fileName);
            StartCoroutine(InitialiseData());
        }
        if (trajectoryType == Trajectory.Predicted && eventType == BrowserEvent.LoadedPredicted){
            // Debug.Log(trajectoryType);
            // Debug.Log(eventType);
            // Debug.Log(filePathPersist);
            folderName = filePathPersist;
            folderName = Application.persistentDataPath;
            inputFile = fileName;
            //inputFile = FileUtilities.RemoveFileExtension(fileName);
            StartCoroutine(InitialiseData());
        }

        if (eventType != BrowserEvent.Opened){
            Debug.Log("browser OApened - no data loaded yet");
        }

    }
    #endregion
    
    #region Events

    public delegate void TrajectoryEvent(Trajectory trajectoryType, Vector3 trajectory);
    public static event TrajectoryEvent OnTrajectoryEvent;

    #endregion


    void Awake(){

    }
    void Start(){
        // StartCoroutine(InitialiseData());
    }

    private IEnumerator InitialiseData(){
        
        yield return new WaitForSeconds(0.5f);
        
        // if (folderName == ""){
        //     //Run CSV Reader
        //     pointList = CSVReader.Read(inputFile);
        // }
        // else{
        //     //Run CSV Reader
        //     pointList = CSVReader.Read(folderName+"/"+inputFile);
        // }

        pointList = CSVReader.Read(inputFile);

        List<string> columnList = new List<string>(pointList[1].Keys);
        
        Debug.Log("There are " + columnList.Count + " columns in the CSV");

        foreach (string key in columnList)
            Debug.Log("Column name is " + key);

        // Assign column names according to index indicated in columnList
        xColumnName = columnList[column1];
        yColumnName = columnList[column2];
        zColumnName = columnList[column3];
        
        // Get maxes of each axis, using FindMaxValue method defined below
        xMax = FindMaxValue(xColumnName);
        yMax = FindMaxValue(yColumnName);
        zMax = FindMaxValue(zColumnName);

        // Get minimums of each axis, using FindMinValue method defined below
        xMin = FindMinValue(xColumnName);
        yMin = FindMinValue(yColumnName);
        zMin = FindMinValue(zColumnName);

        rowCount = pointList.Count;
        
        GetPoints();
        
        //SET DATA IN DAO
        if (trajectoryType == Trajectory.Target){
            DAO.instance.TargetData = vectorData;
        }
        if (trajectoryType == Trajectory.Predicted){
            DAO.instance.PredictedData = vectorData;
        }
        
        //todo verify data loaded via call back or better method
        dataLoaded = true;
    }

    void GetPoints(){
        vectorData = new Vector3[pointList.Count];
        for (var i = 0; i < pointList.Count; i++){
            float x = (Convert.ToSingle(pointList[i][xColumnName]));
            float y = (Convert.ToSingle(pointList[i][yColumnName]));
            float z = (Convert.ToSingle(pointList[i][zColumnName]));
            vectorData[i] = new Vector3(x, y, z);
        }
    }

    void Update(){
        if (dataLoaded){
            if (Input.GetKeyDown(KeyCode.Space) && !streamingData){
                streamingData = true;
                broadcastData = StartCoroutine(BroadcastData());
            }
            if (Input.GetKeyDown(KeyCode.S) && streamingData){
                streamingData = false;
                StopCoroutine(broadcastData);
                if (OnTrajectoryEvent != null){
                    OnTrajectoryEvent(trajectoryType, Vector3.zero);
                }
            }
        }
    }

    private IEnumerator BroadcastData(){
        for (int i = 0; i < vectorData.Length; i++){
            // Debug.Log(trajectoryType.ToString() + ": " +  targetPosition[i]);
            yield return new WaitForFixedUpdate();
            if (OnTrajectoryEvent != null){
                OnTrajectoryEvent(trajectoryType, vectorData[i]);
            }

            if (i >= vectorData.Length-1){
                if (OnTrajectoryEvent != null){
                    OnTrajectoryEvent(trajectoryType, Vector3.zero);
                }
                streamingData = false;
            }
        }

    }
    void FixedUpdate()
    {
        
    }
    
    private float FindMaxValue(string columnName)
    {
        //set initial value to first value
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //Spit out the max value
        return maxValue;
    }

    //Method for finding minimum value, assumes PointList is generated
    private float FindMinValue(string columnName)
    {
        //set initial value to first value
        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }

}
