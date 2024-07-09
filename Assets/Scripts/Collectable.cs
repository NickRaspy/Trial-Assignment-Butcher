using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private Type type;
    [SerializeField] private Material particle;
    [SerializeField] private int points;
    [SerializeField] private GameObject collectableObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<PlayerBehavior>().CurrentScore += points;
            other.transform.parent.GetComponent<PlayerBehavior>().UseEffect(particle, type == Type.Good ? Color.green : Color.red, points);
            GameManager.instance.source.PlayOneShot(soundEffect);
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        collectableObject.transform.Rotate(0f, Time.deltaTime * 10f, 0f);
    }
    [Serializable]
    public enum Type
    {
        Good, Bad
    }
}
