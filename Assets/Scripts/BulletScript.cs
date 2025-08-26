using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Destroy(gameObject, 2f);    
    }

}
