using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] Type type;
    [SerializeField] private int points;
    [SerializeField] private Material particle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<PlayerBehavior>().CurrentScore += points;
            other.transform.parent.GetComponent<PlayerBehavior>().UseEffect(particle, type == Type.Good ? Color.green : Color.red, points);
            GameManager.instance.source.PlayOneShot(soundEffect);
        }
        Destroy(transform.parent.gameObject);
    }
    [Serializable]
    public enum Type
    {
        Good, Bad
    }
}
