using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    float waitTime;
    Animator anim;
    bool miesa = false;
    void Start()
    {
        waitTime = 12f;
        anim = GetComponent<Animator>();
        InvokeRepeating("Animate", waitTime/2, waitTime/2);
    }
    void Animate()
    {
       
        if (!miesa)
        {
            anim.SetBool("miesaj", true);
            miesa = true;
        }
        else
        {
            anim.SetBool("miesaj", false);
            miesa = false;
        }
        
    }
}
