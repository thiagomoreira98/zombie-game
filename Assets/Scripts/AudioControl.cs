using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioSource instance;

    void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
        instance = this.audioSource;
    }
}
