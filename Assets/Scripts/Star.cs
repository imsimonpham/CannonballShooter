using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    [SerializeField] private Renderer _renderer;
        
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _collectSound;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Star"))
        {
            _renderer.enabled = false;
            _audioSource.PlayOneShot(_collectSound);
            Destroy(gameObject, 1f);
        }
    }
}
