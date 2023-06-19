using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private HeroKnight hero;
    [SerializeField] private float radius = 15f;
    [SerializeField] private float speed = 500f;

    // Update is called once per frame
    void Update()
    {
        if (!hero.m_dead)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(hero.gameObject.transform.position, radius, 1 << 9);
            foreach (var enem in colliders)
            {
                if (enem.transform.tag != "Spikes")
                {
                    Vector3 dir = (hero.transform.position - enem.transform.position);
                    if (Mathf.Abs(dir.y) < 2)
                    {
                        dir.Normalize();
                        dir.y = 0;
                        enem.transform.position = Vector3.MoveTowards(enem.transform.position, enem.transform.position + dir, speed * Time.deltaTime);
                        enem.GetComponent<SpriteRenderer>().flipX = dir.x < 0.0f;
                    }
                }
            }
        }
    }
}
