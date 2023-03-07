using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.Utilities;
using Vector3 = UnityEngine.Vector3;

public class UI_ManagerSettings : MonoBehaviour{
    
    //classes
    private DAO dao;
    private FileBrowserCSV fileBrowser;

    [Header("SETTINGS MENU UI")]
    //LOAD BUTTONS
    public TextMeshProUGUI fileDisplayTarget;
    public TextMeshProUGUI fileDisplayPredicted;
    
    //data buttons
    public Image btnBackgroundCoordinate;
    public Image btnBackgroundVelocity;
    
    [Header("SETTINGS MENU UI - Scale Slider")]
    //sliders
    public Slider XYZ_Scale;
    public TextMeshProUGUI XYZ_ScaleText;
    [Header("SETTINGS MENU UI - Offset Slider")]
    //sliders
    public Slider xOffset;
    public Slider yOffset;
    public Slider zOffset;
    public TextMeshProUGUI xSliderText;
    public TextMeshProUGUI ySliderText;
    public TextMeshProUGUI zSliderText;
    
    [Header("SETTINGS MENU UI - Settings Buttons")]
    public Image btnTarget; private bool isOnTarget;
    public Image btnPredicted; private bool isOnPredicted;
    public Image btnBiped; private bool isOnBiped;
    public Image btnHand; private bool isOnHand;
    public Image btnSpheres; private bool isOnSpheres;
    public Image btnScatterplot; private bool isOnScatterPlot;
    
    public Image btnHandIK; private bool isOnHandIK;
    public Image btnFootIK; private bool isOnFootIK;
    
    public Image btnNeuralPathways; private bool isOnNeuralPathways;
    public Image btnLabels; private bool isOnLabels;
    public Image btnEndEffectors; private bool isOnEndEffectors;
    public Image btnTrails; private bool isOnTrails;
    public Image btnEnvironment; private bool isOnEnvironment;
    
    public Image btnUI_Toggle; private bool isOnUI;
    
    //logic
    [Header("SETTINGS Logic")]
    public DataType dataType = Enums.DataType.Velocity;
    private float scale = 1;
    private Vector3 offsets = Vector3.zero;
    
    private float onPosX;
    private float offPosX;
    
    [Header("Read Only - Startup")]
    public TextMeshProUGUI versionNumDisplay;
    
    #region Events

    public delegate void UpdateUI(DataType dataType, float scale, Vector3 offsets);
    public static event UpdateUI OnUpdateUI;

    #endregion

    #region Event Subscriptions

    private void OnEnable(){
        FileBrowserCSV.OnFileBrowserEvent += FileBrowserCSVOnFileBrowserEvent;
    }
    private void OnDisable(){
        FileBrowserCSV.OnFileBrowserEvent -= FileBrowserCSVOnFileBrowserEvent;
    }

    private void FileBrowserCSVOnFileBrowserEvent(BrowserEvent eventType, string filePath, string fileName, string filePathPersist){
        if (eventType == BrowserEvent.LoadedTarget){
            fileDisplayTarget.text = "<b><smallcaps>Target Data: </smallcaps></b>" +fileName;
        }
        if (eventType == BrowserEvent.LoadedPredicted){
            fileDisplayPredicted.text = "<b><smallcaps>Predicted Data: </smallcaps></b>" +fileName;
        }
    }

    #endregion

    #region Initialise

    private void Awake(){
        fileBrowser = gameObject.GetComponent<FileBrowserCSV>(); //file browser is attached to the same game object
        scale = 1;
        offsets = Vector3.zero;
    }

    void Start(){
        dao = DAO.instance;

        XYZ_Scale.onValueChanged.AddListener(delegate {ScaleXYZ(); });
        
        xOffset.onValueChanged.AddListener(delegate {OffsetXYZ(); });
        yOffset.onValueChanged.AddListener(delegate {OffsetXYZ(); });
        zOffset.onValueChanged.AddListener(delegate {OffsetXYZ(); });

        OffsetXYZ();
            
        onPosX = this.transform.position.x;
        offPosX = onPosX-StaticData.instance.menuOffset;
        
        // UI IS SET A FRAME AFTER START TO ALLOW ALL OBJECTS AND INSTANCES TO BE INITIALISED
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart(){
        yield return new WaitForEndOfFrame();
        SetReadOnlyText();
        InitiliseButtons();

        BroadcastUI_Settings();
    }

    #endregion

    private void Update(){
        // xSliderText.text = xOffset.value.ToString("F2");
        // ySliderText.text = yOffset.value.ToString("F2");
        // zSliderText.text = zOffset.value.ToString("F2");

        //TEMP CONTROLS
        // if (Input.GetKeyDown(KeyCode.Alpha1)){
        //     OnButtonToggleUI();
        // }
    }

    #region Data Settings

    public void OpenBrowser(int browser){
        fileBrowser.OpenBrowser(browser);
        
        BroadcastUI_Settings();
    }

    public void DataType(int dataType){
        if (dataType == 1){
            //coordinate
            btnBackgroundCoordinate.color = StaticData.instance.UI_Selected;
            btnBackgroundVelocity.color = StaticData.instance.UI_Default;
            this.dataType = Enums.DataType.Coordinate;
            
        }
        if (dataType == 2){
            //velocity
            btnBackgroundVelocity.color = StaticData.instance.UI_Selected;
            btnBackgroundCoordinate.color = StaticData.instance.UI_Default;
            this.dataType = Enums.DataType.Velocity;
        }
        BroadcastUI_Settings();
    }

    public void ScaleXYZ(){
        // XYZ_ScaleText.text = XYZ_Scale.value.ToString("F2");
        XYZ_ScaleText.text = Mathf.RoundToInt(XYZ_Scale.value).ToString();
        scale = XYZ_Scale.value;
        BroadcastUI_Settings();
    }
    public void OffsetXYZ(){
        xSliderText.text = xOffset.value.ToString("F2");
        ySliderText.text = yOffset.value.ToString("F2");
        zSliderText.text = zOffset.value.ToString("F2");
        offsets = new Vector3(xOffset.value,yOffset.value,zOffset.value);
        BroadcastUI_Settings();
    }

    #endregion

    #region Visual buttons
    
    public void OnButtonTarget(){
        isOnTarget = !isOnTarget;
        btnTarget.color = GetButtonColour(isOnTarget);
        ToggleLayer(EnumTags.Target,isOnTarget);
        ToggleLineRenderer("NeuralPathway", EnumTags.Target, isOnTarget,isOnNeuralPathways);
    }
    public void OnButtonPredicted(){
        isOnPredicted = !isOnPredicted;
        btnPredicted.color = GetButtonColour(isOnPredicted);
        ToggleLayer(EnumTags.Predicted,isOnPredicted);
        ToggleLineRenderer("NeuralPathway", EnumTags.Predicted, isOnPredicted,isOnNeuralPathways);
    }
    
    public void OnButtonBiped(){
        isOnBiped = !isOnBiped;
        btnBiped.color = GetButtonColour(isOnBiped);
        ToggleRenderer("Biped", isOnBiped);
    }
    public void OnButtonHand(){
        isOnHand = !isOnHand;
        btnHand.color = GetButtonColour(isOnHand);
        ToggleRenderer("Hand", isOnHand);
    }
    public void OnButtonSpheres(){
        isOnSpheres = !isOnSpheres;
        btnSpheres.color = GetButtonColour(isOnSpheres);
        ToggleRenderer("TrajectorySphere", isOnSpheres);
        ToggleRenderer("Label", EnumTags.TrajectorySphereLabel, isOnSpheres,isOnLabels);
    }
    public void OnButtonScatterplot(){
        isOnScatterPlot = !isOnScatterPlot;
        btnScatterplot.color = GetButtonColour(isOnScatterPlot);
    }

    #endregion

    #region IK buttons

    public void OnButtonHandIK(){
        isOnHandIK = !isOnHandIK;
        btnHandIK.color = GetButtonColour(isOnHandIK);
        ToggleEffectorIK(EnumTags.EndEffectorHand, isOnHandIK);
    }
    public void OnButtonFootIK(){
        isOnFootIK = !isOnFootIK;
        btnFootIK.color = GetButtonColour(isOnFootIK);
        ToggleEffectorIK(EnumTags.EndEffectorFoot, isOnFootIK);
    }

    #endregion

    #region Display buttons

    public void OnButtonNeuralPathways(){
        isOnNeuralPathways = !isOnNeuralPathways;
        btnNeuralPathways.color = GetButtonColour(isOnNeuralPathways);
        // ToggleRenderer("NeuralPathway", isOnNeuralPathways);
        ToggleRenderer("NeuralPathway", EnumTags.Target, isOnNeuralPathways, isOnTarget);
        ToggleRenderer("NeuralPathway", EnumTags.Predicted, isOnNeuralPathways, isOnPredicted);
    }
    public void OnButtonLabels(){
        isOnLabels = !isOnLabels;
        btnLabels.color = GetButtonColour(isOnLabels);
        ToggleRenderer("Label", isOnLabels);
    }
    public void OnButtonEndEffector(){
        isOnEndEffectors = !isOnEndEffectors;
        btnEndEffectors.color = GetButtonColour(isOnEndEffectors);
        ToggleRenderer("EndEffector", EnumTags.EndEffectorHand, isOnEndEffectors, isOnHandIK);
        ToggleRenderer("EndEffector", EnumTags.EndEffectorFoot, isOnEndEffectors, isOnFootIK);
    }
    public void OnButtonTrails(){
        isOnTrails = !isOnTrails;
        btnTrails.color = GetButtonColour(isOnTrails);
        ToggleRenderer("Trail", isOnTrails);
    }
    public void OnButtonEnvironment(){
        isOnEnvironment = !isOnEnvironment;
        btnEnvironment.color = GetButtonColour(isOnEnvironment);
    }
    
    #endregion

    #region Logic Functions

    private void ToggleRenderer(string tag, bool t){
        GameObject[] go = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in go){
            if (obj.GetComponent<Renderer>()){
                obj.GetComponent<Renderer>().enabled = t;
            }
        }
    }
    private void ToggleRenderer(string tag, EnumTags enumTagEnum, bool t, bool refT){
        GameObject[] go = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in go){
            if (obj.GetComponent<TagEnums>()){
                bool b = obj.GetComponent<TagEnums>().CheckForTag(enumTagEnum);
                if (b && refT){
                    if (obj.GetComponent<Renderer>()){
                        obj.GetComponent<Renderer>().enabled = t;
                    }
                }
            }
        }
    }
    private void ToggleLineRenderer(string tag, EnumTags enumTagEnum, bool t, bool refT){
        GameObject[] go = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in go){
            if (obj.GetComponent<TagEnums>()){
                bool b = obj.GetComponent<TagEnums>().CheckForTag(enumTagEnum);
                if (b && refT){
                    if (obj.GetComponent<LineRenderer>()){
                        obj.GetComponent<LineRenderer>().enabled = t;
                    }
                }
            }
        }
    }
    private void ToggleEffectorIK(EnumTags effectorTag, bool t){
        TagEnums[] effectors = FindObjectsOfType<TagEnums>();
        foreach (TagEnums obj in effectors){
            if (obj.gameObject.GetComponent<MotionTrajectory>()){
                if (effectorTag == EnumTags.EndEffectorHand && obj.CheckForTag(EnumTags.EndEffectorHand) == true){
                    obj.GetComponent<MotionTrajectory>().enabled = t;
                }
                if (effectorTag == EnumTags.EndEffectorFoot && obj.CheckForTag(EnumTags.EndEffectorFoot) == true){
                    obj.GetComponent<MotionTrajectory>().enabled = t;
                }
            }
        }
    }

    private void ToggleLayer(EnumTags layer, bool t){
        Camera cam = Camera.main;
        int layerMask;
        layerMask = 1 << LayerMask.NameToLayer(layer.ToString());
        if (t){
            cam.cullingMask |= layerMask;
        }
        else{
            cam.cullingMask &= ~layerMask;
        }
    }

    #endregion

    #region Control Buttons

    public void OnButtonToggleUI(){
        isOnUI = !isOnUI;
        btnUI_Toggle.color = GetButtonColourGrey(isOnUI);
        StartCoroutine(ToggleUI(isOnUI));
        
        Transform t = btnUI_Toggle.transform.GetChild(0).transform;
        FlipIcon(t, isOnUI);
        
        t.gameObject.GetComponent<Image>().color = GetButtonColour(isOnUI);
    }

    private void FlipIcon(Transform icon, bool flip){
        Vector3 scale = icon.localScale;
        scale.x = -scale.x;
        icon.localScale = scale;
    }
    #endregion

    #region Initialise UI

    private void SetReadOnlyText(){
        versionNumDisplay.text = "Build: " + dao.VersionNumber;
        fileDisplayTarget.text = "<b><smallcaps>Load Target Trajectory</smallcaps></b>";
        fileDisplayPredicted.text = "<b><smallcaps>Load Predicted Trajectory</smallcaps></b>";
    }

    private void InitiliseButtons(){
        //ON
        //set the default datatype 
        DataType(GetDataType(dataType));
        //call the buttons that are on by default
        OnButtonTarget();
        OnButtonPredicted();
        OnButtonBiped();
        OnButtonHand();
        OnButtonSpheres();

        OnButtonNeuralPathways();
        OnButtonLabels();
        OnButtonEndEffector();
        OnButtonTrails();
        
        OnButtonHandIK();
        OnButtonFootIK();
        
        OnButtonEndEffector();
        
        //OFF
        //turn off foot ik and other buttons
        OnButtonFootIK();
        OnButtonHand();
        
        //ON
        //reenable the end effector
        OnButtonEndEffector();

        OnButtonToggleUI();
    }

    #endregion

    #region Send Events

    private void BroadcastUI_Settings(){
        if (OnUpdateUI != null){
            OnUpdateUI(this.dataType, scale, offsets);
        }
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
            this.transform.DOMoveX(onPosX, StaticData.instance.animationDuration*4);
        }
        else{
            this.transform.DOMoveX(offPosX, StaticData.instance.animationDuration*4);
        }
        
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

    private int GetDataType(DataType dType){
        int t = 0;
        if (dType == Enums.DataType.Coordinate){
            t = 1;
        }
        if (dType == Enums.DataType.Velocity){
            t = 2;
        }
        return t;
    }
    private DataType GetDataType(int dType){
        DataType t = 0;
        if (dType == 1){
            t = Enums.DataType.Coordinate;
        }
        if (dType == 2){
            t = Enums.DataType.Velocity;
        }
        return t;
    }

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