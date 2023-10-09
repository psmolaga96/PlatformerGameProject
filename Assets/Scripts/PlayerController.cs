using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rgdBody;
    bool dirToRight = true;
    float horizontalMove;
    public float heroSpeed;
    public float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rgdBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        anim.SetFloat("speed",Mathf.Abs(horizontalMove));
        rgdBody.velocity = new Vector2(horizontalMove*heroSpeed, rgdBody.velocity.y);

        if (horizontalMove<0 && dirToRight)
            {
                Flip();
            }
        if (horizontalMove>0 && !dirToRight)
            {
                Flip();
            }
        if (Input.GetKeyDown(KeyCode.Space))
            {
                rgdBody.AddForce(new Vector2(0f, jumpForce));
                anim.SetTrigger("jump");
            }

    }
    void Flip ()
    {
        dirToRight = !dirToRight;
        Vector3 heroScale = gameObject.transform.localScale;
        heroScale.x *= -1;
        gameObject.transform.localScale = heroScale;
    }

}
