using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegularEnemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    float recoilTimer;

    [SerializeField] protected PlayerController player;
    [SerializeField] protected float speed;

    protected Rigidbody2D rb;

    public virtual void Start()
    {

    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }

    public void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
        }
    }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}