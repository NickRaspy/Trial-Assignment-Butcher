using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Wait());
        if(other.CompareTag("Player"))other.transform.parent.GetComponent<PlayerBehavior>().canMove = false;
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent.GetComponent<PlayerBehavior>().canMove = true;
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        transform.parent.GetComponent<Animator>().Play("GoalOpen");
        GameManager.instance.source.PlayOneShot(clip);
    }
}
