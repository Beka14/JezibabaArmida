using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    float waitTime;
    Animator anim;
    bool bop = false;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = 12f;
        anim = GetComponent<Animator>();
        InvokeRepeating("Animate", waitTime/2, waitTime/2);
    }

    void Animate()
    {
       
        if (!bop)
        {
            anim.SetBool("miesaj", true);
            bop = true;
        }
        else
        {
            anim.SetBool("miesaj", false);
            bop = false;
        }
        
    }
}
