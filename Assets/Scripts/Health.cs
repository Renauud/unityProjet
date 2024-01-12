using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    private bool invincible = false;
    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount)
    {
        if(isDead)
        {
            return;
        }

        if (!invincible)
        {
            currentHealth -= amount; //pb se fait hit plusieurs fois en un seul coup.. il faut sûrement faire en sorte que le hit soit actif que pendant certaines frames de l'attaque et mettre une protection après avoir été hit
            invincible = true;
            StartCoroutine(DelayDamage());
            Debug.Log(gameObject.name + " has been hit");
        }

        if (currentHealth > 0)
        {
            //OnHitWithReference?.Invoke(sender); // utile pour le feedback
        }else
        {
            //OnDeathWithReference?.Invoke(sender);pareil
            isDead = true;
            Debug.Log(gameObject.name + "died !");
            Destroy(gameObject);
        }
    }

    private IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(1);
        invincible = false;
    }
}
