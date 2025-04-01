using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum NANINOVEL_SWITCH
{
    ON,
    OFF
}

public class NaninovelSwitch : MonoBehaviour
{

    private string m_naninovelOnCommand = "@showUI\n @back visible:true";
    private string m_naninovelOffCommand = "@hideUI\n @back visible:false";
    private string m_buttonOnText = "Nani/on";
    private string m_buttonOffText = "Nani/off";
    public NANINOVEL_SWITCH m_isNaninovelActivated = NANINOVEL_SWITCH.OFF;
    private bool m_isUICameraEnabled;
    public GameObject m_UICamera;

    private void Start()
    {
        if (m_isNaninovelActivated == NANINOVEL_SWITCH.ON)
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_buttonOffText;
        else
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_buttonOnText;
    }

    private void Update()
    {
        if(m_UICamera == null)
        {
            m_UICamera = GameObject.Find("UICamera");   //find UI camera for 
            if(m_UICamera != null)
                m_isUICameraEnabled = m_UICamera.GetComponent<Camera>().enabled;
        }

        if (m_UICamera != null  && m_isUICameraEnabled != m_UICamera.GetComponent<Camera>().enabled)
            UpdateSwitchButton();
    }

    public void UpdateSwitchButton()
    {
        if(m_UICamera.GetComponent<Camera>().enabled)
        {
            m_isUICameraEnabled = m_UICamera.GetComponent<Camera>().enabled;
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_buttonOffText;
            m_isNaninovelActivated = NANINOVEL_SWITCH.ON;

        }
        else
        {
            m_isUICameraEnabled = m_UICamera.GetComponent<Camera>().enabled;
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_buttonOnText;
            m_isNaninovelActivated = NANINOVEL_SWITCH.OFF;
        }
    }

    public void ToggleNaninovel()
    {
        if (m_isNaninovelActivated == NANINOVEL_SWITCH.ON)
        {
            this.GetComponent<PlayScript>().SetPlayScriptText(m_naninovelOffCommand);
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_buttonOnText;
            m_isNaninovelActivated = NANINOVEL_SWITCH.OFF;
        }
        else
        {
            this.GetComponent<PlayScript>().SetPlayScriptText(m_naninovelOnCommand);
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_buttonOffText;
            m_isNaninovelActivated = NANINOVEL_SWITCH.ON;
        }
        this.GetComponent<PlayScript>().Play();
    }
}
