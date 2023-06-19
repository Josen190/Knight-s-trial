using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Unit
{
    [SerializeField] int lives = 100000;
    [SerializeField] int damage = 1;
    [SerializeField] bool immortality = true;
    [SerializeField] int price = 100;

    private HeroKnight hero;

    private void Awake()
    {
        GameObject ob = GameObject.Find("Hero");
        hero = ob.GetComponent<HeroKnight>();
    }

    public override void ReceiveDamage(int damage)
    {
        if (!immortality)
            lives -= damage;
        if (lives < 1)
            Die();
        Debug.Log(transform.name + ": " + lives);
    }

    public override void Die()
    {
        hero.score += price;
        Destroy(gameObject);
    }
}
