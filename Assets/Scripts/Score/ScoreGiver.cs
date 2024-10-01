using System;
using UnityEngine;

namespace Butcher_TA
{
    public abstract class ScoreGiver : MonoBehaviour
    {
        [SerializeField] private AudioClip soundEffect;
        [SerializeField] private Type type;
        [SerializeField] private Material particle;
        [SerializeField] private int points;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) ApplyEffect();
        }

        public void ApplyEffect()
        {
            GameManager.instance.Score += points;
            GameManager.instance.player.UseEffect(particle, type == Type.Good ? Color.green : Color.red, points, type == Type.Good);
            GameManager.instance.source.PlayOneShot(soundEffect);

            ExtraAction();
        }

        public abstract void ExtraAction();

        [Serializable]
        public enum Type
        {
            Good, Bad
        }
    }
}
