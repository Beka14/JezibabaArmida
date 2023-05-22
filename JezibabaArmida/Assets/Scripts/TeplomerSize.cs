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
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Debug.Log(screenWidth + " " + screenHeight);
        float targetWidth = (screenHeight * 0.97f)/100;
        float targetHeight = (screenHeight * 0.93f)/100;
        if(targetWidth > 5.7f)
        {
            targetWidth = 5.7f;
            targetHeight = 5.8f;
        }
        Debug.Log(targetWidth+" " + targetHeight);
        Vector3 newScale = new Vector3(targetHeight, targetWidth, transform.localScale.z);
        transform.localScale = newScale;
    }
}
