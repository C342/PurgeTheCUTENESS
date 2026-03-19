using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegularEnemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected PlayerMovement player;
    [SerializeField] protected float speed;

    protected float recoilTimer;
    protected Rigidbody2D rb;

    public virtual void Start()
    {

    }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //player = PlayerMovement.Instance;
    }
}