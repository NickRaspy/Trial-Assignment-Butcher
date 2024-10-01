using TMPro;
using UnityEngine;

namespace Butcher_TA
{
    public class ArrowMultiplierSelect : MonoBehaviour
    {
        [SerializeField] private TMP_Text multiplierText;
        public void SetScoreMultiplier(int multiplier)
        {
            if(GameManager.instance != null)
            {
                GameManager.instance.ScoreMultiplier = multiplier;
                GameManager.instance.SetGetScoreButtonText(true);
                multiplierText.text = "X" + multiplier;
            }
        }
    }
}
