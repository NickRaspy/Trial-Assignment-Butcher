using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Butcher_TA
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private bool isFinal;
        [SerializeField] private int minimumGap;
        private void OnTriggerEnter(Collider other)
        {
            if (GameManager.instance.Score >= minimumGap && !isFinal)
            {
                transform.parent.GetComponent<Animator>().Play("DoorOpen");
                GameManager.instance.source.PlayOneShot(clip);
            }
            else
            {
                GameManager.instance.EndLevel(true);
            }
        }
    }
}
