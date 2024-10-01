using UnityEngine;
using UnityEngine.Splines;

namespace Butcher_TA
{
    public class PlayerFacade : MonoBehaviour
    {
        private IPlayerMovement playerMovement;
        private IPlayerEffects playerEffects;
        private IPlayerScore playerScore;
        private IPlayerAnimation playerAnimation;
        private IPlayerSpline playerSpline;

        private void Awake()
        {
            playerMovement = GetComponent<IPlayerMovement>();
            playerEffects = GetComponent<IPlayerEffects>();
            playerScore = GetComponent<IPlayerScore>();
            playerAnimation = GetComponent<IPlayerAnimation>();
            playerSpline = GetComponent<IPlayerSpline>();
        }

        private void Start()
        {
            GameManager.instance.OnScoreChange.AddListener(OnScoreChange);
        }

        private void Update()
        {
            playerMovement.Move();
        }

        public void SetMoveState(bool isMoving)
        {
            playerMovement.CanMove = isMoving;
            if (isMoving) playerSpline.SetSplinePath(null); // Example usage
            else playerSpline.ResetMove();
        }

        public void UseEffect(Material particle, Color color, int points, bool isGood)
        {
            playerEffects.UseEffect(particle, color, points, isGood);
        }

        public void PlayModelAnimation(string name)
        {
            playerAnimation.PlayModelAnimation(name);
        }

        public void SetModelAnimatorBoolValue(string name, bool value)
        {
            playerAnimation.SetModelAnimatorBoolValue(name, value);
        }

        public void SetSplinePath(SplineContainer spline)
        {
            playerSpline.SetSplinePath(spline);
        }

        public void ResetMove()
        {
            playerSpline.ResetMove();
        }

        public void ResetStartPosition(Transform spawnpoint)
        {
            playerSpline.ResetStartPosition(spawnpoint);
        }

        private void OnScoreChange(int score)
        {
            playerScore.OnScoreChange(score);
            ChangeOutfit(score, true);
        }

        public void ChangeOutfit(int score, bool mustAnimate)
        {
            int currentOutfitIndex = playerScore.GetCurrentOutfitIndex();
            int newOutfitIndex = playerScore.GetNewOutfitIndex(score);

            if (currentOutfitIndex != newOutfitIndex)
            {
                playerScore.ChangeOutfit(score);

                if (mustAnimate)
                {
                    playerAnimation.PlayModelAnimation(currentOutfitIndex < newOutfitIndex ? "Spin" : "Stepped");
                    playerAnimation.SetModelAnimatorBoolValue("walkState", newOutfitIndex > 1);
                }
            }
        }
    }
}
