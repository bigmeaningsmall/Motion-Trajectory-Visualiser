using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Enums;

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
    
    public Image btnNeuroPathways; private bool isOnNeuroPathways;
    public Image btnLabels; private bool isOnLabels;
    public Image btnEndEffectors; private bool isOnEndEffectors;
    public Image btnTrails; private bool isOnTrails;
    public Image btnEnvironment; private bool isOnEnvironment;
    
    //logic
    [Header("SETTINGS Logic")]
    public DataType dataType = Enums.DataType.Velocity;
    private float scale = 1;
    private Vector3 offsets = Vector3.zero;
    
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


    //temp bool
    private bool menuActive = true;
    private void Update(){
        // xSliderText.text = xOffset.value.ToString("F2");
        // ySliderText.text = yOffset.value.ToString("F2");
        // zSliderText.text = zOffset.value.ToString("F2");

        //TEMP CONTROLS
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            if (menuActive){
                // Get all child objects of this game object
                foreach (Transform child in transform){
                    // Deactivate each child object
                    child.gameObject.SetActive(false);
                }
                GetComponent<Image>().enabled = false;
                menuActive = false;
            }
            else{
                // Get all child objects of this game object
                foreach (Transform child in transform){
                    // Deactivate each child object
                    child.gameObject.SetActive(true);
                }
                GetComponent<Image>().enabled = true;
                menuActive = true;
            }
        }
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

    //TODO --- Add an option to toggle target and prtdicted 
    public void OnButtonTarget(){
        isOnTarget = !isOnTarget;
        btnTarget.color = GetButtonColour(isOnTarget);
    }
    public void OnButtonPredicted(){
        isOnPredicted = !isOnPredicted;
        btnPredicted.color = GetButtonColour(isOnPredicted);
    }
    
    public void OnButtonBiped(){
        isOnBiped = !isOnBiped;
        btnBiped.color = GetButtonColour(isOnBiped);
    }
    public void OnButtonHand(){
        isOnHand = !isOnHand;
        btnHand.color = GetButtonColour(isOnHand);
    }
    public void OnButtonSpheres(){
        isOnSpheres = !isOnSpheres;
        btnSpheres.color = GetButtonColour(isOnSpheres);
        ToggleRenderer("TrajectorySphere", isOnSpheres);
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
        ToggleEffector(Effector.Hand, isOnHandIK);
    }
    public void OnButtonFootIK(){
        isOnFootIK = !isOnFootIK;
        btnFootIK.color = GetButtonColour(isOnFootIK);
        ToggleEffector(Effector.Foot, isOnFootIK);
    }

    #endregion

    #region Display buttons

    public void OnButtonNeuroPathways(){
        isOnNeuroPathways = !isOnNeuroPathways;
        btnNeuroPathways.color = GetButtonColour(isOnNeuroPathways);
        ToggleRenderer("Label", isOnNeuroPathways);
    }
    public void OnButtonLabels(){
        isOnLabels = !isOnLabels;
        btnLabels.color = GetButtonColour(isOnLabels);
        ToggleRenderer("Label", isOnLabels);
    }
    public void OnButtonEndEffector(){
        isOnEndEffectors = !isOnEndEffectors;
        btnEndEffectors.color = GetButtonColour(isOnEndEffectors);
        ToggleRenderer("EndEffector", isOnEndEffectors);
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
            obj.GetComponent<Renderer>().enabled = t;
        }
    }
    private void ToggleEffector(Effector effector, bool t){
        TagEffector[] effectors = FindObjectsOfType<TagEffector>();
        foreach (TagEffector obj in effectors){
            if (obj.gameObject.GetComponent<MotionTrajectory>()){
                if (effector == Effector.Hand && obj.effectorTag == Effector.Hand){
                    obj.GetComponent<MotionTrajectory>().enabled = t;
                }
                if (effector == Effector.Foot && obj.effectorTag == Effector.Foot){
                    obj.GetComponent<MotionTrajectory>().enabled = t;
                }
                if (effector == Effector.Head && obj.effectorTag == Effector.Head){
                    obj.GetComponent<MotionTrajectory>().enabled = t;
                }
                if (effector == Effector.Generic && obj.effectorTag == Effector.Generic){
                    obj.GetComponent<MotionTrajectory>().enabled = t;
                }
            }
        }
    }

    #endregion


    #region Initialise UI

    private void SetReadOnlyText(){
        versionNumDisplay.text = "Build: " + dao.VersionNumber;
        fileDisplayTarget.text = "<b><smallcaps>Load Target Trajectory</smallcaps></b>";
        fileDisplayPredicted.text = "<b><smallcaps>Load Predicted Trajectory</smallcaps></b>";
    }

    private void InitiliseButtons(){
        //set the default datatype 
        DataType(GetDataType(dataType));
        //call the buttons that are on by default
        OnButtonTarget();
        OnButtonPredicted();
        OnButtonBiped();
        OnButtonSpheres();
        
        OnButtonHandIK();
        
        OnButtonLabels();
        OnButtonEndEffector();
        OnButtonTrails();
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
    
    #endregion
}