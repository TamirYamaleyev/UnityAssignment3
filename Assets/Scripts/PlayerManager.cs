using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public AudioSource playerHitSFX;

    public float maxHealth = 100f;
    public float currentHealth;

    public Image healthFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHitSFX = GetComponent<AudioSource>();

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        playerHitSFX.Play();
    }

    private void UpdateHealthUI()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }
    }

}
