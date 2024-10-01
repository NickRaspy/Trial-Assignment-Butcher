using ButchersGames;
using System.Collections;
using System.Collections.Generic;
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
            if (instance != null) Destroy(this);
            else instance = this;
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
        [SerializeField] private List<Text> levelNameTexts;

        public bool IsPlaying { get; private set; }

        private int score;
        public int Score
        {
            get => score;
            set
            {
                score = value;

                scoreText.text = value.ToString();

                if(score <= 0) EndLevel(false);

                OnScoreChange.Invoke(value);
            }
        }
        public UnityEvent<int> OnScoreChange { get; set; }

        private Level currentLevel;

        void Start()
        {

            source = player.GetComponent<AudioSource>();
            OnScoreChange = new();

            PrepareGame();
        }

        private void OnGUI()
        {
            if((Event.current.type == EventType.MouseDown || Event.current.type == EventType.TouchDown) && !IsPlaying)
            {
                StartGame();
            }
        }

        public void EndLevel(bool didWin)
        {
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

            foreach (Text text in levelNameTexts)
            {
                text.text = $"Уровень {LevelManager.CurrentLevel}";
            }
        }
        public void StartGame()
        {
            menuUI.gameObject.SetActive(false);
            gameUI.gameObject.SetActive(true);

            /*            player.SetPath(LevelManager.Default.Levels[LevelManager.Default.CurrentLevelIndex].path);*/

            player.SetMoveState(true);

            player.SetModelAnimatorBoolValue("isWalking", true);

            IsPlaying = true;
        }

        private void LoadLevel()
        {
            LevelManager.Default.Init();
            currentLevel = LevelManager.Default.Levels[LevelManager.Default.CurrentLevelIndex];
        }
    }
}
