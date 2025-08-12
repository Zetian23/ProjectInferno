using UnityEngine;
using System.Collections; // This line is necessary for using coroutines and IEnumerator
using System.Collections.Generic; // This line is necessary for using List<T> and other collection types

public enum SoundType
{
    JUMP,
    LAND,
    AK47_FIRE,
    AK47_RELOAD,
    AK47_PICKUP,
    AK47_EMPTY,
    PISTOL_FIRE,
    PISTOL_RELOAD,
    PISTOL_PICKUP,
    PISTOL_EMPTY,
    SHOTGUN_FIRE,
    SHOTGUN_RELOAD,
    SHOTGUN_PICKUP,
    SHOTGUN_EMPTY,
    SPEAR_SLASH,
    SPEAR_THROW,
    SPEAR_PICKUP,
    SPEAR_HIT,
    SPEAR_MISS,
    SPEAR_THROW_HIT,
    SPEAR_THROW_MISS,
    SWORD_SLASH,
    SWORD_PICKUP,
    SWORD_HIT,
    SWORD_MISS,
    SWORD_THROW,
    SWORD_THROW_HIT,
    SWORD_THROW_MISS
    // Add more sound types as needed
}
public class soundmanager : MonoBehaviour
{
    private static soundmanager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       instance = this;
        // Ensure that the sound manager persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public static void PlaySound(/*SoundType, volume*/)
    {
        
    }
}
