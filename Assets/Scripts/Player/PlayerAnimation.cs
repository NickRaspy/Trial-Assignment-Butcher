using UnityEngine;

namespace Butcher_TA
{
    public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
    {
        [SerializeField] private Animator modelAnimator;

        public void PlayModelAnimation(string name) => modelAnimator.Play(name);

        public void SetModelAnimatorBoolValue(string name, bool value) => modelAnimator.SetBool(name, value);

        public void SetModelAnimatorIntegerValue(string name, int value) => modelAnimator.SetInteger(name, value);
    }

    public interface IPlayerAnimation
    {
        void PlayModelAnimation(string name);
        void SetModelAnimatorBoolValue(string name, bool value);
        void SetModelAnimatorIntegerValue(string name, int value);
    }
}