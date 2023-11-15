using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mic : MonoBehaviour
{
    AudioClip record;
    public AudioClip test;
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    public void PlaySnd()
    {
        aud.Play();
    }

    public void RecSnd()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        record = Microphone.Start(Microphone.devices[1].ToString(), false, 3, 44100);

        float[] samples = new float[record.samples];
        record.GetData(samples, 0);
        aud.clip = record;

        AudioClip recording = AudioClip.Create("Á¦¹ß", samples.Length, 1, 44100, false);
        recording.SetData(samples, 0);

        SavWav.Save("C:/Users/Cherry/Documents/GitHub/Elementee/Assets/Cherry/Records/Test", recording);
    }
}
