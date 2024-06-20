using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Audio[] musicAudio, sfxAudio;
    [SerializeField] private AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//Mantenemos el AudioManager durante todo el juego
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string name)
    {
        Audio audio = Array.Find(musicAudio, sound => sound.name == name);

        if (audio != null)
        {
            musicSource.clip = audio.clip;
            musicSource.Play();
        }

    }

    public void PlaySFX(string name)
    {
        Audio audio = Array.Find(sfxAudio, sound => sound.name == name);

        if (audio != null)
        {
            sfxSource.PlayOneShot(audio.clip);
        }

    }

    public void StopAllAudio()
    {
        musicSource.Stop();
        sfxSource.Stop();
    }
}
