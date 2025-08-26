using System;
using UnityEngine;

public class HurtDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("Enemy being hurt");
            GetComponentInParent<Animator>().SetTrigger("Hurt");
            
        }
    }
}
