using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent (typeof(Rigidbody2D))]
public class CBController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 5;
    [SerializeField]
    private float upFactor = 2;
    private Rigidbody2D rb2D;

    private RuntimeAnimatorController animController;
    private Animator anim;

    private int jumpState = 0;
    private double jumpInitYPos;
    private bool falling = false;
    private bool fallen = false;
    private bool jumping = false;
    private bool resettable = false;
    [SerializeField]
    private CBFollower mainCam;
    [SerializeField]
    private AudioClip jumpSound, collideSound, deathSound;
    private AudioSource audio;

    private const int waypointCount = 3;
    [SerializeField]
    private Vector2[] waypoints = new Vector2[waypointCount];
    private int resetPlace = 0;

    private Collider2D[] collidingObjs = new Collider2D[5];

    private bool paused = false;
    [SerializeField]
    private PlayableDirector hotdogTimeline;
    [SerializeField]
    private PlayableDirector milkshakeTimeline;
    [SerializeField]
    private PlayableDirector cheepsTimeline;


    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animController = anim.runtimeAnimatorController;
        audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCam.SetFollow(true, false);
        mainCam.SetDeathAudioDelay(deathSound.length);
        hotdogTimeline.stopped += OnCutsceneOver;
        milkshakeTimeline.stopped += OnCutsceneOver;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Obstacle" || collider.gameObject.tag == "Fallbox" )
        {
            if (!fallen)
            {
                falling = true;
                anim.SetBool("falling", true);

                for (int i=0; i< waypointCount; i++)
                {
                    if (transform.position.x > waypoints[i].x)
                    {
                        resetPlace = i;
                    } else
                    {
                        break;
                    }
                }

                if (collider.gameObject.tag == "Obstacle")
                {
                    rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
                    audio.clip = collideSound;
                    audio.Play();
                }
            }
        }
        if(collider.gameObject.tag == "hotdogTriggerbox")
        {
            collider.gameObject.SetActive(false);
            paused = true;
            mainCam.SetFollow(false, false);
            transform.position = waypoints[1];
            anim.SetBool("falling", false);
            anim.SetBool("fallen", false);
            anim.SetBool("running", false);
            anim.SetBool("jumping", false);
            anim.Play("CB_idle");
            hotdogTimeline.Play();
        }
        if (collider.gameObject.tag == "milkshakeTriggerbox")
        {
            collider.gameObject.SetActive(false);
            paused = true;
            mainCam.SetFollow(false, false);
            transform.position = waypoints[2];
            anim.SetBool("falling", false);
            anim.SetBool("fallen", false);
            anim.SetBool("running", false);
            anim.SetBool("jumping", false);
            anim.Play("CB_idle");

            TimelineAsset timelineAsset = (TimelineAsset)milkshakeTimeline.playableAsset;
            var tracklist = new List<PlayableBinding>(timelineAsset.outputs);
            var track = tracklist[2].sourceObject as TrackAsset;
            milkshakeTimeline.SetGenericBinding(track, anim);

            milkshakeTimeline.Play();
        }
        if (collider.gameObject.tag == "cheepsTriggerbox")
        {
            collider.gameObject.SetActive(false);
            paused = true;
            mainCam.SetFollow(false, false);

            TimelineAsset timelineAsset = (TimelineAsset)cheepsTimeline.playableAsset;
            var tracklist = new List<PlayableBinding>(timelineAsset.outputs);
            var track = tracklist[2].sourceObject as TrackAsset;
            cheepsTimeline.SetGenericBinding(track, anim);

            cheepsTimeline.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Fallbox")
        {
            mainCam.SetFollow(false, true);
            resettable = true;
            audio.clip = deathSound;
            audio.Play();
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
    }

    public IEnumerator AnimationHack()
    {
        yield return new WaitForEndOfFrame();
        anim = gameObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = animController;
    }

    public void OnCutsceneOver(PlayableDirector aDirector)
    {
        if (aDirector == hotdogTimeline || aDirector == milkshakeTimeline)
        {            
            paused = false;
            mainCam.SetFollow(true, false);
            Destroy(GetComponent<Animator>());
            StartCoroutine(AnimationHack());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (resettable)
            {
                // Use SPACEBAR to reset after falling
                if (Input.GetAxis("Jump") > 0)
                {
                    transform.position = new Vector3(waypoints[resetPlace].x, waypoints[resetPlace].y, transform.position.z);
                    rb2D.velocity = new Vector2(0, 0);
                    resettable = false;
                    jumping = false;
                    falling = false;
                    fallen = false;
                    anim.SetBool("falling", false);
                    anim.SetBool("fallen", false);
                    anim.SetBool("running", false);
                    anim.SetBool("jumping", false);
                    anim.Play("CB_idle");
                    mainCam.SetFollow(true, true);
                }
            }
            else
            {
                if (falling)
                {
                    if (rb2D.velocity.magnitude < 0.001)
                    {
                        anim.SetBool("falling", false);
                        anim.SetBool("fallen", true);
                        fallen = true;
                        falling = false;
                        resettable = true;
                        mainCam.SetFollow(false, true);
                        audio.clip = deathSound;
                        audio.Play();
                    }
                }
                else
                {
                    rb2D.AddForce(Vector2.right * 10 * Input.GetAxis("Horizontal"));

                    Vector2 currLocalVelocity = rb2D.velocity;
                    int attachedCollidersCount = rb2D.GetContacts(collidingObjs);

                    for (int i = 0; i < attachedCollidersCount; i++)
                    {
                        if (collidingObjs[i].GetComponent<Rigidbody2D>() && collidingObjs[i].name != name)
                        {
                            currLocalVelocity -= collidingObjs[i].GetComponent<Rigidbody2D>().velocity;
                        }
                    }

                    if (Mathf.Abs(currLocalVelocity.x) > maxSpeed)
                    {
                        rb2D.velocity = new Vector2((Mathf.Sign(rb2D.velocity.x)) * maxSpeed, rb2D.velocity.y);
                    }

                    anim.SetBool("running", Mathf.Abs(currLocalVelocity.x) > 0.001);
                    anim.SetFloat("speedMultiplier", Mathf.Abs(currLocalVelocity.x) / maxSpeed);

                    if (!jumping && Input.GetAxis("Jump") > 0 && (Mathf.Abs(currLocalVelocity.y) < 0.001))
                    {
                        jumpInitYPos = rb2D.position.y;
                        rb2D.AddForce(Vector2.up * upFactor, ForceMode2D.Impulse);
                        jumping = true;
                        anim.SetBool("jumping", true);
                        jumpState = 0;
                        audio.clip = jumpSound;
                        audio.Play();
                        anim.SetInteger("jumpState", jumpState);
                    }
                    else
                    {
                        if (Mathf.Abs(currLocalVelocity.y) > 0.001)
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
                                        if (currLocalVelocity.y < 0)
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
                    }

                    if (currLocalVelocity.magnitude > 0.001)
                    {
                        if (currLocalVelocity.x < 0)
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
    }
}
