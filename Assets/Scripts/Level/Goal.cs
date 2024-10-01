using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Butcher_TA
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(Wait());
            if (other.CompareTag("Player")) GameManager.instance.player.ChangeMove(false);
        }
        private void OnTriggerExit(Collider other)
        {
            other.transform.parent.GetComponent<PlayerBehavior>().ChangeMove(true);
        }
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
            transform.parent.GetComponent<Animator>().Play("GoalOpen");
            GameManager.instance.source.PlayOneShot(clip);
        }
    }
}
