using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enums;
using DG.Tweening;

public class UI_ManagerTrajectoryDisplay : MonoBehaviour{
    public DataType motionType = DataType.Velocity;
    public Trajectory trajectoryType;

    Trajectory trajectoryDisplay;
    Axis axis = Axis.XYZ;

    public Vector3 targetVelocity;
    public Vector3 predictedVelocity;

    private float scale;
    private Vector3 offsets;

    [Header("DATA DISPLAY UI")] public Slider xTargetVelocity;
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

    [Header("Graph Buttons")] private bool graphTargetActive = false;
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

    [Header("Graph Sliders")] public Slider xAxisScale;
    public Slider yAxisScale;
    public TextMeshProUGUI xAxisScaleSliderText;
    public TextMeshProUGUI yAxisScaleSliderText;

    [Header("DISPLAY MENU UI - Control Buttons")]
    public Image btnUI_Toggle;

    private bool isOnUI;

    //privates
    private float onPosX;
    private float offPosX;

    #region Events

    public delegate void UpdateTrajectoryDisplay(bool t, Trajectory trajectoryDisplay);

    public static event UpdateTrajectoryDisplay OnUpdateTrajectoryDisplay;

    public delegate void UpdateTrajectoryAxis(Axis axis);

    public static event UpdateTrajectoryAxis OnUpdateTrajectoryAxis;

    public delegate void UpdateGraphAxisScale(int x, int y);

    public static event UpdateGraphAxisScale OnUpdateGraphAxisScale;

    #endregion

    #region Events Subscriptions

    private void OnEnable(){
        UI_ManagerSettings.OnUpdateUI += UI_ManagerOnUpdateUI;
        DataReader.OnTrajectoryEvent += DataReaderOnTrajectoryEvent;
    }

    private void OnDisable(){
        UI_ManagerSettings.OnUpdateUI -= UI_ManagerOnUpdateUI;
        DataReader.OnTrajectoryEvent -= DataReaderOnTrajectoryEvent;
    }

    private void UI_ManagerOnUpdateUI(DataType dataType, float scale, Vector3 offsets){
        motionType = dataType;
        this.scale = scale;
        this.offsets = offsets;
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

    #region Initialise

    void Start(){
        // xTargetVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // yTargetVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // zTargetVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // xPredictedVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // yPredictedVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });
        // zPredictedVelocity.onValueChanged.AddListener(delegate {UpdateVelocityXYZ(); });

        xAxisScale.onValueChanged.AddListener(delegate{ UpdateAxisScale(); });
        yAxisScale.onValueChanged.AddListener(delegate{ UpdateAxisScale(); });

        UpdateAxisScale();

        ButtonGraphDisplay(1);
        ButtonGraphDisplay(2);
        ButtonGraphAxis(4);

        onPosX = this.transform.position.x;
        offPosX = onPosX+StaticData.instance.menuOffset;

        OnButtonToggleUI();
    }

    #endregion


    private void Update(){
        //TEMP CONTROLS
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            OnButtonToggleUI();
        }
    }

    public void UpdateVelocityXYZ(){
        xTargetVelocity.value = targetVelocity.x * scale;
        yTargetVelocity.value = targetVelocity.y * scale;
        zTargetVelocity.value = targetVelocity.z * scale;
        xPredictedVelocity.value = predictedVelocity.x * scale;
        yPredictedVelocity.value = predictedVelocity.y * scale;
        zPredictedVelocity.value = predictedVelocity.z * scale;
        xTargetVelocitySliderText.text = (xTargetVelocity.value * scale).ToString("f2");
        yTargetVelocitySliderText.text = (yTargetVelocity.value * scale).ToString("f2");
        zTargetVelocitySliderText.text = (zTargetVelocity.value * scale).ToString("f2");
        xPredictedVelocitySliderText.text = (xPredictedVelocity.value * scale).ToString("f2");
        yPredictedVelocitySliderText.text = (yPredictedVelocity.value * scale).ToString("f2");
        zPredictedVelocitySliderText.text = (zPredictedVelocity.value * scale).ToString("f2");
    }

    public void UpdateAxisScale(){
        if (OnUpdateGraphAxisScale != null){
            OnUpdateGraphAxisScale(Mathf.RoundToInt(xAxisScale.value), Mathf.RoundToInt(yAxisScale.value));
        }

        xAxisScaleSliderText.text = Mathf.RoundToInt(xAxisScale.value).ToString();
        yAxisScaleSliderText.text = Mathf.RoundToInt(yAxisScale.value).ToString();
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

    #region Control Buttons

    public void OnButtonToggleUI(){
        isOnUI = !isOnUI;
        btnUI_Toggle.color = GetButtonColourGrey(isOnUI);
        StartCoroutine(ToggleUI(isOnUI));
    }

    #endregion


    #region UI Animation

    IEnumerator ToggleUI(bool t){
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        
        childTransforms = TransformUtilities.RemoveTransformsByTag(childTransforms, "NoScale");
        childTransforms = TransformUtilities.RemoveTransformsByTag(childTransforms, "NoAnimate");
        
        Vector3[] childPositions = new Vector3[childTransforms.Length];
        for (int i = 0; i < childTransforms.Length; i++){
            
            childPositions[i] = childTransforms[i].position;
        }

        if (t){
            this.transform.DOMoveX(onPosX, StaticData.instance.animationDuration * 4);
        }
        else{
            this.transform.DOMoveX(offPosX, StaticData.instance.animationDuration * 4);
        }

        yield return new WaitForFixedUpdate();
        
        for (int i = 0; i < childTransforms.Length; i++){
            if (t){
                childTransforms[i].DOScaleX(1, StaticData.instance.animationDuration);
            }
            else{
                childTransforms[i].DOScaleX(0, StaticData.instance.animationDuration);
            }
        
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    #region Returns

    private Color GetButtonColour(bool t){
        Color c;
        Color d = StaticData.instance.UI_Default;
        Color s = StaticData.instance.UI_Selected;
        if (t){
            c = s;
        }
        else{
            c = d;
        }
        return c;
    }
    private Color GetButtonColourGrey(bool t){
        Color c;
        Color d = StaticData.instance.UI_DefaultGrey;
        Color s = StaticData.instance.UI_SelectedGrey;
        if (t){
            c = s;
        }
        else{
            c = d;
        }
        return c;
    }
    
    #endregion
}