using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAO : MonoBehaviour
{
    public static DAO instance;

    private StaticData staticData;
    
    public string targetFilePath = "";
    public string predictedFilePath = "";
    
    public Vector3[] targetVectorData = new Vector3[0];
    public Vector3[] predictedVectorData = new Vector3[0];

    void Awake(){
        if (instance != null) {
            Destroy(gameObject);
        }else{
            instance = this;
        }
    }
    void Start(){
        staticData = StaticData.instance;
    }
    public string TargetFilePath 
    {
        get { return targetFilePath; }
        set { targetFilePath = value; }
    }
    public string PredictedFilePath 
    {
        get { return predictedFilePath; }
        set { predictedFilePath = value; }
    }

    public Vector3[] TargetData
    {
        get{ return targetVectorData; }
        set{ targetVectorData = value; }
    }

    public Vector3[] PredictedData
    {
        get{ return predictedVectorData; }
        set{ predictedVectorData = value; }
    }
    #region General Config

    /// <summary>
    /// Config details
    /// </summary>
    public string VersionNumber{
        get{ return staticData.versionNumber; }
        set{ staticData.versionNumber = value; }
    }

    #endregion

    
}
