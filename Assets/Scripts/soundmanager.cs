using UnityEngine;
using System.Collections; // This line is necessary for using coroutines and IEnumerator
using System.Collections.Generic; // This line is necessary for using List<T> and other collection types


public enum SoundType
{
    JUMP,
    LAND,
    FOOTSTEP,
    FOOTSTEP_SPRINT,
    HURT,
    DEATH,
    ENEMY_GUN_FIRE,

    WEAPON_PICKUP,
    AK47_FIRE,
    SHOTGUN_FIRE,
    PISTOL_FIRE,
   
    WEAPON_EMPTY,
    WEAPON_RELOAD,
  
    SPEAR_SLASH,
    SPEAR_HIT,
    SWORD_SLASH,
    SWORD_HIT
    // Add more sound types as needed
}

[RequireComponent(typeof(AudioSource))] // Ensure that the GameObject has an AudioSource component

public class soundmanager : MonoBehaviour
{

    [SerializeField] AudioClip[] soundList; // Array to hold sound clips for different sound types

    private static soundmanager instance;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       instance = this;
        // Ensure that the sound manager persists across scenes
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on soundmanager GameObject.");
        }
    }

    // Update is called once per frame
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
