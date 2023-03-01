using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static StaticData instance;
    
    [ShowInInspector]
    public string versionNumber;

    [Header("COLOURS")] 
    public Color32 UI_Default;
    public Color32 UI_Selected;

//******************************
    void Awake(){
        if (instance != null) {
            Destroy(gameObject);
        }else{
            instance = this;
        }
    }
}
