using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Butcher_TA
{
    public class PlayerEffects : MonoBehaviour, IPlayerEffects
    {
        [SerializeField] private List<ParticleSystem> particles;
        [SerializeField] private Animator halo;
        [SerializeField] private TMP_Text pointEffect;

        private SpriteRenderer haloSpriteRenderer;
        private Animator pointEffectAnimator;
        private Coroutine currentCoroutine;

        private void Start()
        {
            haloSpriteRenderer = halo.GetComponent<SpriteRenderer>();
            pointEffectAnimator = pointEffect.GetComponent<Animator>();
        }

        public void UseEffect(Material particle, Color color, int points, bool isGood)
        {
            haloSpriteRenderer.color = color;
            halo.gameObject.SetActive(false);
            halo.gameObject.SetActive(true);
            halo.Play("Splash");

            foreach (var p in particles)
            {
                p.GetComponent<ParticleSystemRenderer>().material = particle;
                p.Play();
            }

            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(Combo(points, isGood));
        }

        private IEnumerator Combo(int points, bool isGain)
        {
            int combo = 0;
            combo += isGain ? points : -points;
            pointEffect.text = (isGain ? "+" : "") + combo;

            pointEffectAnimator.Play(isGain ? "Gain" : "Loss");

            yield return new WaitForSeconds(1f);

            combo = 0;
        }
    }

    public interface IPlayerEffects
    {
        void UseEffect(Material particle, Color color, int points, bool isGood);
    }
}
