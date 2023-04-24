using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoAudioManager : MonoBehaviour
{
    AudioSource source;
    Button button;
    bool playing;
    Image i;
    void Start()
    {
        source = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        i = GetComponent<Image>();
        button.onClick.AddListener(PlayAudio);
    }

    private void Update()
    {
        if (!source.isPlaying) i.color = new Color(255, 255, 255, 255);
    }
    void PlayAudio()
    {
        if (!playing)
        {
            playing = true;
            i.color = new Color(0, 250, 0, 255);
            source.Play();
        }

        else
        {
            playing = false;
            i.color = new Color(255, 255, 255, 255);
            source.Stop();
        }
    }
}
