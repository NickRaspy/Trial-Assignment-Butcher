using ButchersGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private void Awake()
    {
        if(instance == null) instance = this;
        else if(instance == this) Destroy(gameObject);
    }
    [SerializeField] PlayerBehavior player;
    public AudioSource source;
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip loseSFX;
    [Header("UI")]
    [SerializeField] private Canvas gameUI;
    [SerializeField] private Canvas menuUI;
    [SerializeField] private Canvas winUI;
    [SerializeField] private Canvas loseUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private List<Text> levelNameTexts;

    public bool gameStarted = false;
    void Start()
    {
        source = player.GetComponent<AudioSource>();
        StartGame();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameStarted)
        {
            StartMove();
        }
        scoreText.text = player.CurrentScore.ToString();
    }
    public void EndLevel(bool didWin)
    {
        player.StopMoving();
        
        gameUI.gameObject.SetActive(false);
        if (didWin) 
        {
            player.model.GetComponent<Animator>().Play("Dance");
            source.PlayOneShot(winSFX);
            winUI.gameObject.SetActive(true);
        }
        else
        {
            player.model.GetComponent<Animator>().Play("Anger");
            source.PlayOneShot(loseSFX);
            loseUI.gameObject.SetActive(true);
        }
    }
    public void StartGame()
    {
        player.CurrentScore = 40f;
        gameStarted = false;
        player.model.GetComponent<Animator>().SetBool("isWalking", false);
        player.model.GetComponent<Animator>().Play("Idle");
        LevelManager.Default.Init();
        player.transform.SetLocalPositionAndRotation(LevelManager.Default.Levels[LevelManager.Default.CurrentLevelIndex].playerSpawnPoint.position,
    LevelManager.Default.Levels[LevelManager.Default.CurrentLevelIndex].playerSpawnPoint.rotation);
        foreach (Text text in levelNameTexts)
        {
            text.text = $"Уровень {LevelManager.CurrentLevel}";
        }
    }
    public void StartMove()
    {
        menuUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        player.canMove = true;
        player.pathFollower.pathCreator = LevelManager.Default.Levels[LevelManager.Default.CurrentLevelIndex].path;
        player.model.GetComponent<Animator>().SetBool("isWalking", true);
        gameStarted = true;
    }
}
