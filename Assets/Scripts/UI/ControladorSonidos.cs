using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorSonidos : MonoBehaviour
{
    public static ControladorSonidos Instance;
    private AudioSource audioSource, loopAudioSource;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        loopAudioSource = gameObject.AddComponent<AudioSource>();
        loopAudioSource.loop = true;
        loopAudioSource.volume = 0.1f;
    }

    public void EjecutarSonido(AudioClip sonido)
    {
        audioSource.PlayOneShot(sonido);
    }
    public void ReproducirSonidoEnLoop(AudioClip sonido)
    {
        if (loopAudioSource.clip != sonido)
        {
            loopAudioSource.clip = sonido;
            loopAudioSource.Play();
        }
    }

    public void DetenerSonidoEnLoop()
    {
        loopAudioSource.Stop();
        loopAudioSource.clip = null;
    }
}
