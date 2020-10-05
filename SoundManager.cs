using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource EffectsSource;
    public AudioSource MusicSource;

    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    // Singleton instance.
    public static SoundManager Instance = null;


    public AudioClip bonusFallDver;
    public AudioClip bonusSuperCatch;
    public AudioClip endGame;
    public AudioClip shot;
    public AudioClip hubBort;
    public AudioClip goal;
    public AudioClip shtanga;
    public AudioClip svistok;
    public AudioClip tolpa;
    public AudioClip newlevel;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    // Play a single clip through the sound effects source.
    public void Play(AudioClip clip)
    {
        //EffectsSource.clip = clip;
        EffectsSource.PlayOneShot(clip);
    }
    public void MuteOn(bool todo)
    {

        if (todo)
        {
            Saves.inst.OPTION_MUTE = true;
            PlayerPrefs.SetInt("OPTION_MUTE", 1);
        }
        else
        { 
            Saves.inst.OPTION_MUTE = false;
            PlayerPrefs.SetInt("OPTION_MUTE", 0);
        }

        AudioListener.volume = Saves.inst.OPTION_MUTE ? 0 : 1;
    }
    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }
    public void StopMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Stop();
    }
    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clips[randomIndex];
        EffectsSource.Play();
    }

}