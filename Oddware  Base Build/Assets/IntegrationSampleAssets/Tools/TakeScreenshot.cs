using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeScreenshot : MonoBehaviour
{
    [SerializeField] private InputActionReference screenshotInput;

    [SerializeField] private int _screenWidth = 1920;
    [SerializeField] private int _screenHeight = 1080;
    [SerializeField] private int _screenStartX = 0;
    [SerializeField] private int _screenStartY = 0;

    private void OnEnable()
    {
        screenshotInput.action.started += RunScreenshotSequence;
    }
    
    private void OnDisable()
    {
        screenshotInput.action.started -= RunScreenshotSequence;
    }

    [ContextMenu("Take Screenshot")]
    private void ScreenshotContext()
    {
        StartCoroutine(CoroutineScreenshot());
    }
    private void RunScreenshotSequence(InputAction.CallbackContext obj)
    {
        StartCoroutine(CoroutineScreenshot());
    }

    private IEnumerator CoroutineScreenshot()
    {
        yield return new WaitForEndOfFrame();

        int width = _screenWidth;
        int height = _screenHeight;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, _screenStartX, _screenStartY);
        screenshotTexture.Apply();
        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenShot.png", byteArray);
    }
}
