using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource BGM_AudioSource;
    [SerializeField]
    AudioSource SE_AudioSource;

    [SerializeField]
    List<BGMSoundData> bGM_SoundDatas;
    [SerializeField]
    List<SESoundData> se_SoundDatas;

    public float masterVolume = 1;
    public float bgmMasterVolume = 1;
    public float seMasterVolume = 1;
	public static SoundManager Instance { get; private set; }

	private void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}

    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        BGMSoundData data = bGM_SoundDatas.Find(data => data.bgm == bgm);
        BGM_AudioSource.clip = data.audioClip;
        BGM_AudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        BGM_AudioSource.Play();
    }

    public void PlaySE(SESoundData.SE se)
    {
        SESoundData data = se_SoundDatas.Find(data => data.se == se);
        SE_AudioSource.volume=data.volume*seMasterVolume*masterVolume;
        SE_AudioSource.PlayOneShot(data.audioClip);
    }

	[System.Serializable]
    public class BGMSoundData
    {
        public enum BGM
        {
            Title,
            Dungeon,
            MiddiumBoss,
            Boss,
            //‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
            Hoge
        }
        public BGM bgm;
        public AudioClip audioClip;
        [Range(0,1)]
        public float volume;
    }

    [System.Serializable]
    public class SESoundData
	{
        public enum SE
        {
            PlayerAttack,
            GolemAttack,
            DemonAttack,
            Damage,
            Death,
            //‚±‚ê‚ªƒ‰ƒxƒ‹‚É‚È‚é
            Hoge,
        }
        public SE se;
        public AudioClip audioClip;
        [Range(0,1)]
        public float volume;
    }
}
