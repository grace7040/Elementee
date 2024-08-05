using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instacne;

    public Sound[] Sfx = null;
    public Sound[] Bgm = null;
    public Sound[] DefaultSfx = null;

    [SerializeField] AudioSource BgmPlayer = null;
    [SerializeField] AudioSource[] SfxPlayer = null;

    int _customSoundCnt = 2; //UI_SoundCustom의 커스텀 soundObjects개수
    string _audioDir = "/audios";

    private void Awake()
    {
        if (Instacne == null)
        {
            Instacne = this;
            DontDestroyOnLoad(Instacne);
        }
        else
            Destroy(gameObject);

        _audioDir = Application.persistentDataPath + _audioDir;
    }
    private void Start()
    {
        if (Directory.Exists(_audioDir))
            LoadAudios();
        else
        {
            Directory.CreateDirectory(_audioDir);
            SaveAudios();
        }

        PlayBGM("mainBGM");
    }
    public void PlayBGM(string bgmName)
    {
        for (int i = 0; i < Bgm.Length; i++)
        {
            if (bgmName == Bgm[i].Name)
            {
                BgmPlayer.clip = Bgm[i].Clip;
                BgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        BgmPlayer.Stop();
    }

    public void PlaySFX(string sfxName)
    {
        for (int i = 0; i < Sfx.Length; i++)
        {
            if (sfxName == Sfx[i].Name)
            {
                for (int j = 0; j < SfxPlayer.Length; j++)
                {
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!SfxPlayer[j].isPlaying)
                    {
                        SfxPlayer[j].clip = Sfx[i].Clip;
                        SfxPlayer[j].Play();
                        return;
                    }
                }
                //Debug.Log("모든 오디오 플레이어가 재생중입니다.");
                return;
            }
        }
        Debug.Log(sfxName + " 이름의 효과음이 없습니다.");
        return;
    }

    public void SetSFX(string sfxName, AudioClip clip)
    {
        for (int i = 0; i < Sfx.Length; i++)
        {
            if (sfxName == Sfx[i].Name)
            {
                Sfx[i].Clip = clip;
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
        for (int i = 0; i < _customSoundCnt; i++)
        {
            SavWav.Save(_audioDir + "/" + Sfx[i].Name, Sfx[i].Clip);
        }
    }

    IEnumerator TestUnityWebRequest()
    {
        for (int i = 0; i < _customSoundCnt; i++)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(_audioDir + "/" + Sfx[i].Name + ".wav", AudioType.WAV))
            {
                /* 기존코드
                yield return www.Send();
                if (www.isNetworkError){ Debug.Log(www.error); }
                 */

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Sfx[i].Clip = DownloadHandlerAudioClip.GetContent(www);
                    Sfx[i].Clip.name = Sfx[i].Name;
                }

            }
        }
    }

}