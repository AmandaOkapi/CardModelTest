using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMono : MonoBehaviour, IGridObjectAppearance
{
    public CardData cardData;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float destructionTime;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
