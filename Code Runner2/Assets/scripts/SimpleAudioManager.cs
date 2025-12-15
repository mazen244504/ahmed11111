
using UnityEngine;

public class SimpleAudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public float volume = 0.3f;

    private AudioSource audioSource;

    void Start()
    {

        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = true;

 
        audioSource.Play();

        Debug.Log("Background music started!");
    }


    public void ChangeMusic(AudioClip newMusic)
    {
        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.Play();
    }
}