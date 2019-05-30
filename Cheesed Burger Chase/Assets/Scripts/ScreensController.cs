using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScreensController : MonoBehaviour
{
    [SerializeField]
    private Sprite splashScreen, storyScreen, instructionScreen, creditsScreen, deathScreen, winScreen;
    private SpriteRenderer spriteRenderer;
    public enum ScreenState { none, splash, story, intro, credits, death, win };
    private ScreenState screenState = ScreenState.splash;
    private ScreenState preCreditsState = ScreenState.none;
    private float currCountdownTime = 3.0f;
    [SerializeField]
    private CBController cheesyController;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = splashScreen;
        cheesyController.paused = true;
    }

    public IEnumerator DelayedScreenCoroutine(ScreenState newState, float delay)
    {
        yield return new WaitForSeconds(delay);

        SetScreen(newState);

        yield return null;
    }

    public void SetScreenDelayed(ScreenState newState, float delay)
    {
        StartCoroutine(DelayedScreenCoroutine(newState, delay));
    }

    public void SetScreen(ScreenState newState)
    {
        screenState = newState;
        switch (screenState)
        {
            case ScreenState.none:
                {
                    currCountdownTime = 0.0f;
                    spriteRenderer.sprite = null;
                }
                break;
            case ScreenState.splash:
                {
                    currCountdownTime = 3.0f;
                    spriteRenderer.sprite = splashScreen;
                }
                break;
            case ScreenState.story:
                {
                    currCountdownTime = 0.5f;
                    spriteRenderer.sprite = storyScreen;
                }
                break;
            case ScreenState.intro:
                {
                    currCountdownTime = 0.5f;
                    spriteRenderer.sprite = instructionScreen;
                }
                break;
            case ScreenState.credits:
                {
                    currCountdownTime = 0f;
                    spriteRenderer.sprite = creditsScreen;
                }
                break;
            case ScreenState.death:
                {
                    currCountdownTime = 0f;
                    spriteRenderer.sprite = deathScreen;
                }
                break;
            case ScreenState.win:
                {
                    currCountdownTime = 0f;
                    spriteRenderer.sprite = winScreen;
                }
                break;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (screenState)
        {
            case ScreenState.none:
                {

                } break;
            case ScreenState.splash:
                {
                    currCountdownTime -= Time.deltaTime;
                    if (currCountdownTime <= 0)
                    {
                        screenState = ScreenState.story;
                        spriteRenderer.sprite = storyScreen;
                        currCountdownTime = 0.5f;
                    }
                } break;
            case ScreenState.story:
                {
                    currCountdownTime -= Time.deltaTime;
                    if (currCountdownTime <= 0)
                    {
                        if (Input.GetKeyUp(KeyCode.Space))
                        {
                            screenState = ScreenState.intro;
                            spriteRenderer.sprite = instructionScreen;
                            currCountdownTime = 0.5f;
                        }
                    }

                } break;
            case ScreenState.intro:
                {
                    currCountdownTime -= Time.deltaTime;
                    if (currCountdownTime <= 0)
                    {
                        if (Input.GetKeyUp(KeyCode.Space))
                        {
                            screenState = ScreenState.none;
                            spriteRenderer.sprite = null;
                            currCountdownTime = 0f;
                            cheesyController.paused = false;
                        }
                    }

                }
                break;
            case ScreenState.credits:
                {
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        SetScreen(preCreditsState);
                    }

                }
                break;
            case ScreenState.death:
                {
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        screenState = ScreenState.none;
                        spriteRenderer.sprite = null;
                        cheesyController.paused = false;
                        cheesyController.UnDie();
                    }
                } break;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && (screenState != ScreenState.none) && (screenState != ScreenState.splash) && (screenState != ScreenState.credits))
        {
            preCreditsState = screenState;
            screenState = ScreenState.credits;
            spriteRenderer.sprite = creditsScreen;
        }
    }
}
