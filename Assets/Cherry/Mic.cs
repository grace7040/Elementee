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
        SavWav.Save("C:/Users/user/wkspaces/Elementee/Assets/Cherry/Records/Test", aud.clip);
    }

    public void RecSnd()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        record = Microphone.Start(Microphone.devices[0].ToString(), false, 1, 44100);
        aud.clip = record;

        //float[] samples = new float[record.samples];
        //record.GetData(samples, 0);
        

        //AudioClip recording = AudioClip.Create("제발", samples.Length, 1, 44100, false);
        //recording.SetData(samples, 0);

        


        // 데탑용 path : "C:/Users/Cherry/Documents/GitHub/Elementee/Assets/Cherry/Records/Test"
    }
}
