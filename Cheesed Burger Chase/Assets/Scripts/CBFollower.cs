using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBFollower : MonoBehaviour
{
    [SerializeField]
    private Transform followee;
    private bool follow = true;
    private AudioSource audio;
    private float deathDelay = 0;
    [SerializeField]
    private AudioClip mainTune, deathTune, winTune;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (follow)
        {
            transform.position = new Vector3(followee.position.x, followee.position.y, transform.position.z);
        }
    }

    public void SetDeathAudioDelay(float delay)
    {
        deathDelay = delay;
    }

    public void SetFollow(bool newValue, bool died)
    {
        if (follow ^ newValue && died)
        {
            if (!newValue)
            {
                audio.Stop();
                audio.clip = deathTune;
                audio.PlayDelayed(deathDelay);
            } else
            {
                audio.Stop();
                audio.clip = mainTune;
                audio.Play();
            }
        }
        follow = newValue;
    }

    public void SetWin()
    {
        SetFollow(false, false);
        audio.Stop();
        audio.clip = winTune;
        audio.Play();
    }
}
