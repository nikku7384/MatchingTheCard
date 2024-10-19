using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton pattern for global access
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private List<AudioClip> audioClips;  // List of audio clips
    private Dictionary<string, AudioClip> clipDictionary; // Dictionary for quick access

    private AudioSource audioSource;     // Audio source to play clips

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize audio source
        audioSource = GetComponent<AudioSource>();

        // Initialize dictionary
        clipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
        {
            if (!clipDictionary.ContainsKey(clip.name))
            {
                clipDictionary.Add(clip.name, clip);
            }
        }
    }

    // Method to play sound by clip name
    public void PlaySound(string clipName)
    {
        if (clipDictionary.ContainsKey(clipName))
        {
            audioSource.PlayOneShot(clipDictionary[clipName]);
        }
        else
        {
            Debug.LogWarning("Sound clip not found: " + clipName);
        }
    }

    // Method to stop the currently playing sound
    public void StopSound()
    {
        audioSource.Stop();
    }

    // Method to change volume
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp(volume, 0f, 1f);  // Clamps between 0 and 1
    }
}
