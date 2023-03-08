using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static StaticData instance;

    [Header("CONFIG")]
    public int fixedFrameRate = 60;
    public string versionNumber;

    [Header("ANIMATION")] 
    public float animationDuration;
    public float buttonShakeAmount;
    public float buttonShakeStrength;
    public float cameraMoveOffset; //amount the camera can move each time hoz or vert per 
    public int menuOffset;
    
    
    [Header("COLOURS")] 
    public Color32 UI_Default;
    public Color32 UI_Selected;
    public Color32 UI_SelectedTarget;
    public Color32 UI_SelectedPredicted;
    public Color32 UI_DefaultGrey;
    public Color32 UI_SelectedGrey;
    public Color32 UI_White = Color.white;

//******************************
    void Awake(){
        if (instance != null) {
            Destroy(gameObject);
        }else{
            instance = this;
        }
    }
}
