using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    public void PlaySound(AudioClip clip)
    {
        GameManager.instance.source.PlayOneShot(clip);
    }
}
