using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] private List<AudioSource> soundEffects = new List<AudioSource>();
    public void PlaySFX(int index) {
        soundEffects[index].Play();
    }
}
