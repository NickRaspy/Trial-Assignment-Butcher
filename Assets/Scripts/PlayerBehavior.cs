using PathCreation.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public PathFollower pathFollower;
    public Transform model;
    public Animator halo;
    public ParticleSystem particles;
    public List<GameObject> outfits;
    [SerializeField] private Animator gain;
    [SerializeField] private Animator loss;
    [SerializeField] private Image sliderFill;
    [SerializeField] private Slider slider;
    [SerializeField] private Text stateText;
    [Header("Colors")]
    [SerializeField]List<Color> stateColors;
    [Header("Values")]
    [SerializeField] List<string> states;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float borderLimit = 1f;
    private int outfitLevel = 1;
    public bool canMove;
    private float currentScore = 40f;
    private int combo = 0;
    private Coroutine currentCoroutine;
    public float CurrentScore
    {
        get { return currentScore; }
        set { if (!GameManager.instance.gameStarted) return; 
            currentScore = Mathf.Clamp(value, 0, 140);
            slider.value = currentScore;
            int currentOutfitLevel = outfitLevel;
            if (currentScore < 35) outfitLevel = 0;
            else if (currentScore >= 35 && currentScore < 70) outfitLevel = 1;
            else if (currentScore >= 70 && currentScore < 105) outfitLevel = 2;
            else if (currentScore >= 105 && currentScore < 140) outfitLevel = 3;
            else if (currentScore >= 140) outfitLevel = 4;
            if(currentScore <= 0)
            {
                GameManager.instance.EndLevel(false);
            }
            if(currentOutfitLevel != outfitLevel)
            {
                if (currentOutfitLevel < outfitLevel) model.GetComponent<Animator>().Play("Spin");
                else model.GetComponent<Animator>().Play("Stepped");
                outfits[currentOutfitLevel].SetActive(false);
                outfits[outfitLevel].SetActive(true);
                sliderFill.color = stateColors[outfitLevel];
                stateText.text = states[outfitLevel];
                stateText.color = stateColors[outfitLevel];
                if (outfitLevel > 1) model.GetComponent<Animator>().SetInteger("walkState", 1);
                else model.GetComponent<Animator>().SetInteger("walkState", 0);
            }
        }
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && canMove)
        {
            model.Translate(Input.GetAxis("Mouse X") * Time.deltaTime * speed, 0f, 0f);
        }
        model.localPosition = new Vector3(Mathf.Clamp(model.localPosition.x, -borderLimit, borderLimit), 0f, 0f);
    }
    public void StopMoving()
    {
        pathFollower.pathCreator = null;
        pathFollower.distanceTravelled = 0f;
        canMove = false;
    }
    public void UseEffect(Material particle, Color color, int points)
    {
        halo.GetComponent<SpriteRenderer>().color = color;
        halo.Play("Splash");
        if(particles.GetComponent<ParticleSystemRenderer>() != null) particles.GetComponent<ParticleSystemRenderer>().material = particle;
        particles.Play();
        if(currentCoroutine != null)StopCoroutine(currentCoroutine);
        if (color == Color.green) currentCoroutine = StartCoroutine(Combo(points, true));
        else currentCoroutine = StartCoroutine(Combo(points, false));
    }
    public void StandartOutfit()
    {
        for(int i = 0; i < outfits.Count; i++)
        {
            if (i != 1) outfits[i].SetActive(false);
            else outfits[i].SetActive(true);
        }
    }
    private IEnumerator Combo(int points, bool isGain)
    {
        combo += points;
        if (isGain)
        {
            gain.GetComponent<TextMeshProUGUI>().text = $"+{combo}$";
            gain.Play("Gain");
            yield return new WaitForSeconds(1f);
        }
        else
        {
            combo = 0;
            combo -= points;
            loss.GetComponent<TextMeshProUGUI>().text = $"{combo}$";
            loss.Play("Loss");
            yield return new WaitForSeconds(1f);
        }
        combo = 0;
    }
}
