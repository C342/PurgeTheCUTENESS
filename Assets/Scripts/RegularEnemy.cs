using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegularEnemy : MonoBehaviour
{
    [SerializeField] protected float health;

    [SerializeField] protected PlayerController player;
    [SerializeField] protected float speed;

    protected Rigidbody2D rb;

    public virtual void Start()
    {

    }

    private void Update()
    {
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

        public void EnemyHit(float _damageDone)
        {
            health -= _damageDone;
        }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}