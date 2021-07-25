using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{

    public static SoundController instance { private set; get; }

    public GameObject SoundPrefab;

    [Header("Music Holder")]
    public Transform MusicHolder;

    [Header("Effect Holder")]
    public Transform EffectHolder;

    [Header("Volumes")]
    public Scrollbar ScrollVolMaster;
    public Scrollbar ScrollVolMusic;
    public Scrollbar ScrollVolEffect;


    private float volMaster;
    private float volMusic;
    private float volEffect;

    private AudioSource[] soundsMusic;
    private AudioSource[] soundsEffect;


    private const string keyVolMaster = "volMaster";
    private const string keyVolMusic = "volMusic";
    private const string keyVolEffect = "volEffect";
    private const float volDefaultValue = 1f;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        Setup();

    }

    public void Setup()
    {
       
        var effectClips = Resources.LoadAll<AudioClip>("Sounds/Effects");
        soundsEffect = new AudioSource[effectClips.Length];

        for (int i =0; i < effectClips.Length; i++)
        {
            var audioSource = Instantiate(SoundPrefab, EffectHolder).GetComponent<AudioSource>();
            audioSource.clip = effectClips[i];
            audioSource.gameObject.name = effectClips[i].name;
            soundsEffect[i] = audioSource;
        }

        if (soundsMusic == null || soundsMusic.Length == 0)
            soundsMusic = MusicHolder.GetComponentsInChildren<AudioSource>();

       // LoadVolConfig(ref volMaster, ref ScrollVolMaster, keyVolMaster);
      //  LoadVolConfig(ref volMusic, ref ScrollVolMusic, keyVolMusic);
      //  LoadVolConfig(ref volEffect, ref ScrollVolEffect, keyVolEffect);



        void LoadVolConfig(ref float vol, ref Scrollbar bar, string key)
        {
            if (PlayerPrefs.HasKey(key))
                vol = PlayerPrefs.GetFloat(key);
            else
            {
                PlayerPrefs.SetFloat(key, volDefaultValue);
                vol = volDefaultValue;
                PlayerPrefs.Save();
            }

            bar.value = vol;
            bar.onValueChanged.RemoveAllListeners();
            bar.onValueChanged.AddListener((float val) => UpdateAllSoundConfig());

        }


        UpdateAllSoundConfig();
    }

    public void UpdateAllSoundConfig()
    {
      //  volMaster = ScrollVolMaster.value;
   //     volMusic = ScrollVolMusic.value;
   //     volEffect = ScrollVolEffect.value;

 //       SaveConfig(volMaster, keyVolMaster);
 //       SaveConfig(volMusic, keyVolMusic);
 //       SaveConfig(volEffect, keyVolEffect);

       /* for (int i = 0; i < soundsMusic.Length; i++)
        {
            soundsMusic[i].volume = 0.5f;
        }*/

        for (int i = 0; i < soundsEffect.Length; i++)
        {
            soundsEffect[i].volume = 0.5f;
        }

        void SaveConfig(float vol, string key)
        {
            PlayerPrefs.SetFloat(key, vol);
            PlayerPrefs.Save();
        }
    }


    public void PlayAudioEffect(string name, SoundAction action = SoundAction.Play)
    {
        for (int i = 0; i < soundsEffect.Length; i++)
        {
            var effect = soundsEffect[i];
            if (effect.name == name)
            {
                if (action == SoundAction.Play)
                {
                    if (!effect.isPlaying)
                    {
                        effect.Play();
                    }
                }
                else if (action == SoundAction.Stop)
                    effect.Stop();

            }
        }
    }




}

public enum SoundAction
{
    Play,
    Stop,
}