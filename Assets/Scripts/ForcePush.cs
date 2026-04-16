using System;
using UnityEngine;

public class ForcePush : MonoBehaviour
{
    public float pushAmount;
    public float pushRadius;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Push();
        }
    }

    private void Push()
    {
        Collider2D colliders = Physics2D.OverlapCircle(transform.position, pushRadius);

        //foreach (Collider2D pushedObject in colliders)
        //{
        //    if (pushedObject.CompareTag("Push"))
        //    {

        //    }
        //}
    }
}