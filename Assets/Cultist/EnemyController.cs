using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Damageable))]
public class EnemyController : MonoBehaviour
{
    public Animator anim;
    public float movementSpeed;
    public Damageable damageable;
    public Transform groundCheck;
    public Transform wallCheck;
    public float groundCheckRadious = 0.1f;
    public float wallCheckRadious = 0.1f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;
    public Rigidbody2D rb;
    public Transform playerCheckPos;
    public Vector2 playerCheckSize = new Vector2(0.5f, 0.05f);
    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }
  

    private void Update()
    {
        if(!Physics2D.OverlapCircle(groundCheck.position, groundCheckRadious, groundLayer))
        {
            Flip();
        }
        if (Physics2D.OverlapCircle(wallCheck.position, wallCheckRadious, wallLayer))
        {
            Flip();
        }

        anim.SetFloat("speed", Mathf.Abs(movementSpeed));
        if (Physics2D.OverlapBox(playerCheckPos.position, playerCheckSize, 0, playerLayer))
        {
            anim.SetTrigger("attack");

        }
    }
    private void FixedUpdate()
    {
        if (IsAlive)
        {
            if (!damageable.LockVelocity)
            {
                transform.Translate(Time.deltaTime * movementSpeed * transform.right);
            }
        }
    }
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        movementSpeed *= -1;
    }
    public bool IsAlive
    {
        get
        {
            return anim.GetBool("isAlive");
        }
    }
    public void OnHit(int damage, Vector2 knockback) {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
