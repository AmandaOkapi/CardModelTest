using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassMono : MonoBehaviour, IGridObjectAppearance
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float destructionTime;


    public void Die(){
        animator.SetTrigger("Die");
        audioSource.Play();
        StartCoroutine(DestroyAfterAnimation());
    }


    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destructionTime);
        Destroy(gameObject);
    }
}
