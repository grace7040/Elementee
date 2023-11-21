using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instacne;

    int soundObjectsNum = 2; //UI_SoundCustom의 커스텀 soundObjects개수

    public Sound[] sfx = null;
    public Sound[] bgm = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer = null;

    string audioDir = "/audios";

    private void Awake()
    {
        if (Instacne == null)
        {
            Instacne = this;
            DontDestroyOnLoad(Instacne);
        }
        else
            Destroy(gameObject);

        var obj = FindObjectsOfType<AudioManager>();
        if (obj.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        string dir = Application.persistentDataPath + audioDir;
        if (Directory.Exists(dir))
            LoadAudios();
        else
        {
            Directory.CreateDirectory(dir);
            SaveAudios();
        }
    }
    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 오디오 플레이어가 재생중입니다.");
                return;
            }
        }
        Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        return;
    }

    public void SetSFX(string p_sfxName, AudioClip p_clip)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                sfx[i].clip = p_clip;
            }
        }
    }

    //WAV to AudioClip
    void LoadAudios() //Application.persistentDataPath에 저장된 사운드 로드
    {
        StartCoroutine(nameof(TestUnityWebRequest));
    }

    //AudioClip to WAV
    public void SaveAudios()
    {
        string dir = Application.persistentDataPath + audioDir;
        for (int i = 0; i < soundObjectsNum; i++)
        {
            SavWav.Save(dir + "/"+ sfx[i].name, sfx[i].clip);
        }
    }

    IEnumerator TestUnityWebRequest()
    {
        string dir = Application.persistentDataPath + audioDir;
        for (int i = 0; i < soundObjectsNum; i++)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(dir + "/" + sfx[i].name + ".wav", AudioType.WAV))
            {
                yield return www.Send();

                if (www.isNetworkError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    sfx[i].clip = DownloadHandlerAudioClip.GetContent(www);
                    sfx[i].clip.name = sfx[i].name;
                }

            }
        }
        //완료했다고 알려주기. 오브젝트 매니저한테도 알려주기

    }


}
