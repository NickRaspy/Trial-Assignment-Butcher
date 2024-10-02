using ButchersGames;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Butcher_TA
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public PlayerBehavior player;

        [Header("Audio")]
        public AudioSource source;
        [SerializeField] private AudioClip winSFX;
        [SerializeField] private AudioClip loseSFX;

        [Header("UI")]
        [SerializeField] private Canvas gameUI;
        [SerializeField] private Canvas menuUI;
        [SerializeField] private Canvas winUI;
        [SerializeField] private Canvas loseUI;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text summaryScoreText;
        [SerializeField] private TMP_Text getScoreButtonText;
        [SerializeField] private TMP_Text getMultiScoreButtonText;
        [SerializeField] private List<Text> levelNameTexts;

        public bool IsPlaying { get; private set; }

        private int score;
        public int Score
        {
            get => score;
            set
            {
                if (score != value)
                {
                    score = value;
                    UpdateScoreUI();
                    if (score <= 0) EndLevel(false);
                    OnScoreChange.Invoke(value);
                }
            }
        }

        public UnityEvent<int> OnScoreChange { get; private set; } = new UnityEvent<int>();

        private int summaryScore = 0;
        public int SummaryScore
        {
            get => summaryScore;
            set
            {
                if (summaryScore != value)
                {
                    summaryScore = value;
                    summaryScoreText.text = value.ToString();
                }
            }
        }

        public int ScoreMultiplier { get; set; }

        private Level currentLevel;

        void Start() => PrepareGame();

        private void OnGUI()
        {
            if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.TouchDown) && !IsPlaying)
            {
                StartGame();
            }
        }

        public void EndLevel(bool didWin)
        {
            SetGetScoreButtonText(false);
            player.SetMoveState(false);
            gameUI.gameObject.SetActive(false);
            player.PlayModelAnimation(didWin ? "Dance" : "Anger");
            source.PlayOneShot(didWin ? winSFX : loseSFX);
            if (didWin) winUI.gameObject.SetActive(true);
            else loseUI.gameObject.SetActive(true);
        }

        public void PrepareGame()
        {
            Score = 40;
            IsPlaying = false;
            player.SetModelAnimatorBoolValue("isWalking", false);
            player.PlayModelAnimation("Idle");
            player.ChangeOutfit(Score, false);
            LoadLevel();
            player.ResetStartPosition(currentLevel.playerSpawnPoint);
            player.SetSplinePath(currentLevel.spline);
            player.ResetMove();
            UpdateLevelNameTexts();
        }

        public void StartGame()
        {
            menuUI.gameObject.SetActive(false);
            gameUI.gameObject.SetActive(true);
            player.SetMoveState(true);
            player.SetModelAnimatorBoolValue("isWalking", true);
            IsPlaying = true;
        }

        public void SetSummaryScore(bool isMultiplied) => SummaryScore += Score * (isMultiplied ? ScoreMultiplier : 1);

        public void SetGetScoreButtonText(bool isMultiplied)
        {
            if (!isMultiplied) getScoreButtonText.text = Score.ToString();
            else getMultiScoreButtonText.text = (Score * ScoreMultiplier).ToString();
        }

        private void LoadLevel()
        {
            LevelManager.Default.Init();
            currentLevel = LevelManager.Default.Levels[LevelManager.Default.CurrentLevelIndex];
        }

        private void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
        }

        private void UpdateLevelNameTexts()
        {
            foreach (Text text in levelNameTexts)
            {
                text.text = $"Уровень {LevelManager.CurrentLevel}";
            }
        }
    }
}