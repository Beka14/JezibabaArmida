using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip asponjedenkamen;
    [SerializeField] AudioClip dobrapraca;
    [SerializeField] AudioClip editor1;
    [SerializeField] AudioClip gratulujem;
    [SerializeField] AudioClip lentakdalej;
    [SerializeField] AudioClip mozesprejstnadalsiu;
    [SerializeField] AudioClip musisnajstvsetkyodpovede;
    [SerializeField] AudioClip nemasspravnuteplotu;
    [SerializeField] AudioClip niecosiprehliadol;
    [SerializeField] AudioClip odomkol2;
    [SerializeField] AudioClip odomkol3;
    [SerializeField] AudioClip skontrolujsito;
    [SerializeField] AudioClip skontrolujsiteplotu;
    [SerializeField] AudioClip skustoesteraz;
    [SerializeField] AudioClip super;
    [SerializeField] AudioClip totorieseniesiuzzadal;
    [SerializeField] AudioClip unlockeditor1;

    AudioSource source;

    public void PlaySound(int i)
    {
        source = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        switch (i)
        {
            case 0:
                source.clip = asponjedenkamen;
                break;
            case 1:
                source.clip = dobrapraca;
                break;
            case 2:
                source.clip = editor1;
                break;
            case 3:
                source.clip = gratulujem;
                break;
            case 4:
                source.clip = lentakdalej;
                break;
            case 5:
                source.clip = mozesprejstnadalsiu;
                break;
            case 6:
                source.clip = musisnajstvsetkyodpovede;
                break;
            case 7:
                source.clip = nemasspravnuteplotu;
                break;
            case 8:
                source.clip = niecosiprehliadol;
                break;
            case 9:
                source.clip = odomkol2;
                break;
            case 10:
                source.clip = odomkol3;
                break;
            case 11:
                source.clip = skontrolujsito;
                break;
            case 12:
                source.clip = skontrolujsiteplotu;
                break;
            case 13:
                source.clip = skustoesteraz;
                break;
            case 14:
                source.clip = super;
                break;
            case 15:
                source.clip = totorieseniesiuzzadal;
                break;
            case 16:
                source.clip = unlockeditor1;
                break;
            default:
                break;
        }
        source.Play();
    }
}
