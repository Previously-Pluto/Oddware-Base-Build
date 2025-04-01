using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openMenuBar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuBar;  
    private bool toggleBool;

    void Start()
    {
        
    }

    // Update is called once per frame
 void Update ()    {
         if (Input.GetKeyDown(KeyCode.JoystickButton4)) {
            toggleBool = !toggleBool;
            menuBar.SetActive(toggleBool);     
            Debug.Log("test");
         }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key was pressed.");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Space key was released.");
        }
  }

}
