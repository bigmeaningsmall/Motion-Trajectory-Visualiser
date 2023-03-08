using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_ManagerControlPanel : MonoBehaviour{
    
    [Header("SCRIPT COMPONENTS UI")]
    public Canvas UI_Canvas;
    public UI_ManagerSettings UI_Settings;
    public UI_ManagerTrajectoryDisplay UI_TrajectoryDisplay;
    
    [Header("SCRIPT COMPONENTS CAMERA")]
    public MoveAroundObject cameraOrbit;
    public CameraLookSetter cameraLookSetter;

    [Header("CONTROL BUTTONS")]
    private bool isOnControlPanel = true;
    private bool isOnCanvas = true;

    public Image btnStreaming;
    private bool isOnStreaming;
    public Image btnResetEffector;
    private bool isOnResetEffector;
    public Image btnPlay;
    private bool isOnPlay;
    public Image btnStop;
    private bool isOnStop;
    public Image btnExit;
    private bool isOnExit;

    #region Events

    public delegate void ResetEffectors();

    public static event ResetEffectors OnResetEffectors;

    #endregion

    void Start(){
        //initialise buttonsa
        OnButtonStop();
    }

    // Update is called once per frame
    void Update(){
        #region Buttons

        //buttons....
        if (Input.GetKeyDown(KeyCode.S)){
            OnButtonStreaming();
        }

        if (Input.GetKeyDown(KeyCode.R)){
            OnButtonResetEffector();
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            OnButtonPlay();
        }

        if (Input.GetKeyDown(KeyCode.X)){
            OnButtonStop();
        }

        if (Input.GetKeyDown(KeyCode.Return)){
            OnButtonExit();
        }

        #endregion


        #region UI

        //UI........
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            UI_Settings.OnButtonToggleUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){
            UI_TrajectoryDisplay.OnButtonToggleUI();
        }

        if (Input.GetKeyDown(KeyCode.H)){
            OnButtonToggleCanvas();
        }

        if (Input.GetKeyDown(KeyCode.C)){
            OnButtonToggleControlPanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }

        #endregion

        #region Camera

        //CAMERA........
        if (Input.GetKeyDown(KeyCode.F)){
            cameraOrbit.OrbitFreezeToggle();
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            cameraLookSetter.CameraUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            cameraLookSetter.CameraDown();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            cameraLookSetter.CameraLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            cameraLookSetter.CameraRight();
        }

        #endregion

    }

    #region Control Buttons

    public void OnButtonStreaming(){
        isOnStreaming = !isOnStreaming;
        Transform t = btnStreaming.transform.GetChild(0).transform;
        t.gameObject.GetComponent<Image>().color = GetButtonColour(isOnStreaming);
    }

    public void OnButtonResetEffector(){
        isOnResetEffector = !isOnResetEffector;
        // Transform t = btnResetEffector.transform.GetChild(0).transform;
        // t.gameObject.GetComponent<Image>().color = GetButtonColour(isOnResetEffector);
        if (OnResetEffectors != null){
            OnResetEffectors();
        }
    }

    public void OnButtonPlay(){
        isOnPlay = !isOnPlay;
        Transform t = btnPlay.transform.GetChild(0).transform;
        t.gameObject.GetComponent<Image>().color = GetButtonColour(isOnPlay);
    }

    public void OnButtonStop(){
        isOnStop = !isOnStop;
        // Transform s = btnStop.transform.GetChild(0).transform;
        // s.gameObject.GetComponent<Image>().color = GetButtonColour(isOnStop);
    }

    public void OnButtonExit(){
        isOnExit = !isOnExit;
        Transform t = btnExit.transform.GetChild(0).transform;
        t.gameObject.GetComponent<Image>().color = GetButtonColour(isOnExit);

        ReloadCurrentScene();
    }

    //CONTROLS - As button functions if  used in future - for now key controls
    public void OnButtonToggleCanvas(){
        isOnCanvas = !isOnCanvas;
        UI_Canvas.enabled = isOnCanvas;
    }

    public void OnButtonToggleControlPanel(){
        isOnControlPanel = !isOnControlPanel;
        this.gameObject.GetComponent<Image>().enabled = isOnControlPanel;
        ToggleRenderer(isOnControlPanel);
    }

    #endregion

    #region Logic Functions

    private void ToggleRenderer(bool t){
        Transform[] childList = GetComponentsInChildren<Transform>();
        foreach (Transform child in childList){
            if (child.GetComponent<Renderer>()){
                child.GetComponent<Renderer>().enabled = t;
            }
            if (child.GetComponent<Image>()){
                child.GetComponent<Image>().enabled = t;
            }
            if (child.GetComponent<TextMeshProUGUI>()){
                child.GetComponent<TextMeshProUGUI>().enabled = t;
            }
            if (child.GetComponent<LineRenderer>()){
                child.GetComponent<LineRenderer>().enabled = t;
            }
        }
    }
    
    private void ReloadCurrentScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion

    #region Returns

    private Color GetButtonColour(bool t){
        Color c;
        Color d = StaticData.instance.UI_White;
        Color s = StaticData.instance.UI_SelectedTarget;
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