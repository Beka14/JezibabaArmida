using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfettiAnimator : MonoBehaviour
{
    Animator anim;
    bool bop = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator Animate()
    {
        Image i = GetComponent<Image>();
        anim.SetBool("confetti", true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        yield return new WaitForSeconds(2);
        anim.SetBool("confetti", false);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
    }
}
