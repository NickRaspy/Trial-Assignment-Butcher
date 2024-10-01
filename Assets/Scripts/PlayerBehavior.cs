using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

namespace Butcher_TA
{
    public class PlayerBehavior : MonoBehaviour
    {
        [Header("Path")]
        [SerializeField] private SplineAnimate splineAnimate;

        [Header("Control and Model")]
        [SerializeField] private Transform control;
        [SerializeField] private Transform modelPoint;
        [SerializeField] private Animator modelAnimator;
        [SerializeField] private List<Outfit> outfits;

        [Header("Effects")]
        [SerializeField] private List<ParticleSystem> particles;
        [SerializeField] private Animator halo;
        [SerializeField] private TMP_Text pointEffect;

        [Header("Score Objects")]
        [SerializeField] private Image sliderFill;
        [SerializeField] private Slider slider;
        [SerializeField] private Text stateText;

        [Header("Colors")]
        [SerializeField] List<Color> stateColors;

        [Header("Values")]
        [SerializeField] List<string> states;
        [SerializeField] private float speed = 1f;
        [SerializeField] private float borderLimit = 1f;
        [SerializeField] private float rotationAngle = 15f;
        [SerializeField] private float returnSpeed = 2f; 

        public bool CanMove { get; set; }

        private int combo = 0;

        private Coroutine currentCoroutine;

        private int currentOutfitIndex;

        private void Start()
        {
            GameManager.instance.OnScoreChange.AddListener(score => 
            {
                SetScoreSliderValue(score);
                if(score > 0) ChangeOutfit(score, true);
            });
        }

        private void Update()
        {
            if (CanMove)
            {
                if (Input.GetMouseButton(0))
                {
                    float moveX = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
                    control.Translate(moveX, 0f, 0f);
                    RotateCharacter(Input.mousePosition);
                }
                else
                {
                    ReturnToOriginalRotation();
                }
            }

            control.localPosition = new Vector3(Mathf.Clamp(control.localPosition.x, -borderLimit, borderLimit), 0f, 0f);
        }

        private void RotateCharacter(Vector3 mousePosition)
        {
            Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
            float offsetX = viewportPosition.x - 0.5f;
            float targetAngle = Mathf.Clamp(offsetX * 2, -rotationAngle, rotationAngle);

            modelPoint.localRotation = Quaternion.Euler(0f, targetAngle * 100f, 0f);
        }

        private void ReturnToOriginalRotation()
        {
            modelPoint.localRotation = Quaternion.Lerp(modelPoint.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
        }

        public void SetMoveState(bool isMoving)
        {
            CanMove = isMoving;
            if (isMoving) splineAnimate.Play();
            else splineAnimate.Pause();
        }

        public void UseEffect(Material particle, Color color, int points, bool isGood)
        {
            halo.GetComponent<SpriteRenderer>().color = color;
            halo.gameObject.SetActive(false);
            halo.gameObject.SetActive(true);
            halo.Play("Splash");

            particles.ForEach(p => 
            {
                p.GetComponent<ParticleSystemRenderer>().material = particle;
                p.Play();
            });

            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(Combo(points, isGood));
        }

        private void SetScoreSliderValue(int score) => slider.value = score;

        public void PlayModelAnimation(string name) => modelAnimator.Play(name);

        public void SetModelAnimatorBoolValue(string name, bool value) => modelAnimator.SetBool(name, value);

        public void SetSplinePath(SplineContainer spline) => splineAnimate.Container = spline;

        public void ResetMove() => splineAnimate.Restart(false);

        public void ResetStartPosition(Transform spawnpoint) => transform.SetLocalPositionAndRotation(spawnpoint.position, spawnpoint.rotation);

        public void ChangeOutfit(int score, bool mustPlayAnimation)
        {

            int index = outfits.FindLastIndex(x => score >= x.minimalScore);

            if (currentOutfitIndex == index) return;

            if (mustPlayAnimation) modelAnimator.Play(currentOutfitIndex < index ? "Spin" : "Stepped");

            outfits[currentOutfitIndex].outfitModel.SetActive(false);

            currentOutfitIndex = index;

            outfits[currentOutfitIndex].outfitModel.SetActive(true);

            ChangeStatus(currentOutfitIndex);

            modelAnimator.SetInteger("walkState", currentOutfitIndex > 1 ? 1 : 0);
        }

        private void ChangeStatus(int index)
        {
            sliderFill.color = stateColors[index];
            stateText.text = states[index];
            stateText.color = stateColors[index];
        }

        private IEnumerator Combo(int points, bool isGain)
        {
            combo += isGain ? points : -points;
            pointEffect.text = (isGain ? "+" : "") + combo;

            pointEffect.GetComponent<Animator>().Play(isGain ? "Gain" : "Loss");

            yield return new WaitForSeconds(1f);

            combo = 0;
        }
    }
}
