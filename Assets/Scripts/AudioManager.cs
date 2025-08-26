using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip Level01Music;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayMusic(Level01Music, 0.2f);        
    }

    public void PlayMusic(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
    {
        musicSource.loop = true;
        musicSource.volume = volume;
        musicSource.clip = clip;
        musicSource.Play();        
    }

    public void PlaySFXOneShot(AudioClip clip, float volume = 1.0f)
    {
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySFXCustom(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
    {
        if (clip == null) return;
        
        if (pitch == 0.0f) return;

        GameObject sfxObj = new GameObject("SFX");
        
        AudioSource sfxAudioSource = sfxObj.AddComponent<AudioSource>();
        sfxAudioSource.clip = clip;
        sfxAudioSource.volume = volume;
        sfxAudioSource.pitch = pitch;
        
        if (pitch < 0.0f)
            sfxAudioSource.time = sfxAudioSource.clip.length - 0.01f; // Start from the end of the clip
        
        sfxAudioSource.Play();
        
        float adjustedDuration = clip.length / Mathf.Abs(pitch);
        Destroy(sfxObj, adjustedDuration);
    }

}
