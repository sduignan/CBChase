using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class CBController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 5;
    [SerializeField]
    private float upFactor = 2;
    private Rigidbody2D rb2D;
    private Animator anim;
    private int jumpState = 0;
    private double jumpInitYPos;
    private bool falling = false;
    private bool fallen = false;
    private bool jumping = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!fallen)
        {
            falling = true;
            anim.SetBool("falling", true);
            rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (falling) {
            if (rb2D.velocity.magnitude < 0.001)
            {
                anim.SetBool("falling", false);
                anim.SetBool("fallen", true);
                fallen = true;
                falling = false;
            }
        } else if (fallen)
        {
            // Use SPACEBAR to reset after falling
            if (Input.GetAxis("Jump") > 0)
            {
                fallen = false;
                anim.SetBool("fallen", false);
                anim.SetBool("running", false);
                anim.SetBool("jumping", false);
                anim.Play("CB_idle");
            }
        }
        else
        {
            rb2D.AddForce(Vector2.right * 10 * Input.GetAxis("Horizontal"));
            if (Mathf.Abs(rb2D.velocity.x) > maxSpeed)
            {
                rb2D.velocity = new Vector2((Mathf.Sign(rb2D.velocity.x)) * maxSpeed, rb2D.velocity.y);
            }
            anim.SetBool("running", Mathf.Abs(rb2D.velocity.x) > 0.001);
            anim.SetFloat("speedMultiplier", Mathf.Abs(rb2D.velocity.x) / maxSpeed);

            if (!jumping && Input.GetAxis("Jump") > 0 && (Mathf.Abs(rb2D.velocity.y) < 0.001))
            {
                jumpInitYPos = rb2D.position.y;
                rb2D.AddForce(Vector2.up * upFactor, ForceMode2D.Impulse);
                jumping = true;
                anim.SetBool("jumping", true);
                jumpState = 0;
            }

            if (Mathf.Abs(rb2D.velocity.y) > 0.001)
            {
                switch (jumpState)
                {
                    case 0:
                        {
                            if (rb2D.position.y > jumpInitYPos + 0.1)
                            {
                                jumpState++;
                            }
                        }
                        break;
                    case 1:
                        {
                            if (rb2D.velocity.y < 0)
                            {
                                jumpState++;
                            }
                        }
                        break;
                    case 2:
                        {
                            if (rb2D.position.y < jumpInitYPos + 0.1)
                            {
                                jumpState++;
                            }
                        }
                        break;
                    case 3:
                        {
                            // No transitions other than to stop jumping
                        }
                        break;
                }

                anim.SetInteger("jumpState", jumpState);
            }
            else
            {
                jumpState = 3;
                anim.SetInteger("jumpState", jumpState);
                anim.SetBool("jumping", false);
                jumping = false;
            }

            if (rb2D.velocity.magnitude > 0.001)
            {
                if (rb2D.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }
}
