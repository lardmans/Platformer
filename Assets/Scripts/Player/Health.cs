using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField]
    AudioClip[] audioClipsDamaged, audioClipsDied;

    [SerializeField]
    float maxHealth = 100f;

    float currentHealth;
    AudioSource audioSource;
    public bool alive;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        alive = currentHealth > 0;
    }

    public void TakeDamage(float damage)
    {
        if (! alive)
        {
            Debug.Log("Player is dead");
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (audioSource != null && audioClipsDamaged.Length > 0)
            {
                AudioClip randomClip = audioClipsDamaged[Random.Range(0, audioClipsDamaged.Length)];
                audioSource.PlayOneShot(randomClip);
            }
        }
    }

    void Die()
    {
        if (audioSource != null && audioClipsDied.Length > 0)
        {
            AudioClip randomClip = audioClipsDied[Random.Range(0, audioClipsDied.Length)];
            audioSource.PlayOneShot(randomClip);
        }

        alive = false;

        Player player = GetComponent<Player>();

        if (player != null)
        {
            player.Die();
        }
    }

    public void Regenerate()
    {
        currentHealth = maxHealth;
        alive = true;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
