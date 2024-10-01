using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Butcher_TA
{
    public class PlayerScore : MonoBehaviour, IPlayerScore
    {
        [SerializeField] private List<Outfit> outfits;
        [SerializeField] private Image sliderFill;
        [SerializeField] private Slider slider;
        [SerializeField] private Text stateText;
        [SerializeField] private List<Color> stateColors;
        [SerializeField] private List<string> states;

        private int currentOutfitIndex;

        private void Start()
        {
            GameManager.instance.OnScoreChange.AddListener(OnScoreChange);
        }

        public void OnScoreChange(int score)
        {
            SetScoreSliderValue(score);
        }

        public void ChangeOutfit(int score)
        {
            int index = GetNewOutfitIndex(score);

            if (currentOutfitIndex == index) return;

            outfits[currentOutfitIndex].outfitModel.SetActive(false);

            currentOutfitIndex = index;

            outfits[currentOutfitIndex].outfitModel.SetActive(true);

            ChangeStatus(currentOutfitIndex);
        }

        public int GetCurrentOutfitIndex()
        {
            return currentOutfitIndex;
        }

        public int GetNewOutfitIndex(int score)
        {
            return outfits.FindLastIndex(x => score >= x.minimalScore);
        }

        private void SetScoreSliderValue(int score) => slider.value = score;

        private void ChangeStatus(int index)
        {
            sliderFill.color = stateColors[index];
            stateText.text = states[index];
            stateText.color = stateColors[index];
        }
    }

    public interface IPlayerScore
    {
        void OnScoreChange(int score);
        void ChangeOutfit(int score);
        int GetCurrentOutfitIndex();
        int GetNewOutfitIndex(int score);

    }
}
