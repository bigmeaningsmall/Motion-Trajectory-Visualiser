using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_ManagerControlPanel : MonoBehaviour{

    public Canvas UI_Canvas;
    public UI_ManagerSettings UI_Settings;
    public UI_ManagerTrajectoryDisplay UI_TrajectoryDisplay;
    
    private bool isOnCanvas;
    
    public Image btnStreaming; private bool isOnStreaming;
    public Image btnResetEffector; private bool isOnResetEffector;
    public Image btnPlay; private bool isOnPlay;
    public Image btnStop; private bool isOnStop;
    public Image btnExit; private bool isOnExit;
    
    #region Events

    public delegate void ResetEffectors();
    public static event ResetEffectors OnResetEffectors;

    #endregion
    
    void Start()
    {
        //initialise buttonsa
        OnButtonStop();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            UI_Settings.OnButtonToggleUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            UI_TrajectoryDisplay.OnButtonToggleUI();
        }
        
        if (Input.GetKeyDown(KeyCode.H)){
            isOnCanvas = !isOnCanvas;
            UI_Canvas.enabled = isOnCanvas;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
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

    #endregion

    #region Logic Functions

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