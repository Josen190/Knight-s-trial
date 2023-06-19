 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private int lives = 1;              //жизни
    private int damage = 1;             //урон
    private bool immortality = true;    //бессмертие
    private int price = 1;

    public virtual void ReceiveDamage(int damage)
    {
        if (!immortality)
            lives -= damage;
        if (lives < 1)
            Die();
        Debug.Log(transform.name + ": " + lives);
    }

    public virtual void Die()
    {
        Destroy(gameObject); 
    }

    public int GetDamege()
    {
        return damage;
    }

    public int GetLives()
    {
        return lives;
    }

    public int GetPrice()
    {
        return price;
    }

}
