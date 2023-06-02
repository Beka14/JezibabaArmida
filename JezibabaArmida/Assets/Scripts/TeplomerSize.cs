using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeplomerSize : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("ScaleObject");
    }

    IEnumerator ScaleObject()
    {
        yield return new WaitForEndOfFrame();

        float screenRatio = 2690f/Screen.width;


        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float a = (screenRatio * 0.18f);
        float b = (Screen.width < 2160)? a+0.05f:a;
        float c = Screen.width * b;
        float targetWidth = c / 100f; //((screenHeight * ((screenRatio * 0.34f)+0.1f)) / 100)+0.4f;
        float targetHeight = (c / 100f)-0.2f; //((screenHeight * ((screenRatio * 0.34f)+0.1f)) / 100);
        /*
        if (targetWidth > 5.7f)
        {
            targetWidth = 5.7f;
            targetHeight = 5.8f;
        }
        */
        Vector3 newScale = new Vector3(targetHeight, targetWidth, transform.localScale.z);
        transform.localScale = newScale;
        
    }
}
