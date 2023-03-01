using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAO : MonoBehaviour
{
    public static DAO instance;

    private StaticData staticData;
    
    public string targetFilePath = "";
    public string predictedFilePath = "";

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
