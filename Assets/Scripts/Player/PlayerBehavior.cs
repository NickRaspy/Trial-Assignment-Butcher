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
            GameManager.instance.OnScoreChange.AddListener(score => 
            {
                if (score > 0) ChangeOutfit(score, true);
                else GameManager.instance.EndLevel(false);
            });
        }

        public void SetMoveState(bool isMoving)
        {
            playerMovement.CanMove = isMoving;
            if(isMoving ) playerSpline.StartMove();
            else playerSpline.StopMove();
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

        public void ChangeOutfit(int score, bool mustAnimate)
        {
            int currentOutfitIndex = playerScore.GetCurrentOutfitIndex();
            int newOutfitIndex = playerScore.GetNewOutfitIndex(score);

            if (currentOutfitIndex != newOutfitIndex)
            {
                playerScore.ChangeOutfit(score);

                if (!mustAnimate) return;
                playerAnimation.PlayModelAnimation(currentOutfitIndex < newOutfitIndex ? "Spin" : "Stepped");
                playerAnimation.SetModelAnimatorIntegerValue("walkState", newOutfitIndex > 1 ? 1 : 0);
            }
        }
        public void ChangeMove(bool isMoving)
        {
            playerMovement.CanMove = isMoving;
        }
    }
}