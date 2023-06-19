using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKnight : Unit
{
    [SerializeField] int lives = 5;
    [SerializeField] int damage = 1;
    [SerializeField] bool immortality = false;
    [SerializeField] int price = 1;
    [SerializeField] private AudioClip soundOfGettingHit;

    private HeroKnight hero;

    private void Awake()
    {
        GameObject ob = GameObject.Find("Hero");
        hero = ob.GetComponent<HeroKnight>();
    }

    public override void ReceiveDamage(int damage)
    {
        Vector2 dir = (this.transform.position - hero.transform.position).normalized;
        this.GetComponent<Rigidbody2D>().AddForce(dir * 6.5f, ForceMode2D.Impulse);
        AudioSource.PlayClipAtPoint(soundOfGettingHit, transform.position);

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
