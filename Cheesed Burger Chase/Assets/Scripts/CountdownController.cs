using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    [SerializeField]
    private Sprite[] numberSprites = new Sprite[10];
    [SerializeField]
    private Transform trackee;
    [SerializeField]
    private float targetX;
    private SpriteRenderer[] digitRenderers = new SpriteRenderer[3];
    private int[] digitStates = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        digitStates[0] = 3;
        digitStates[1] = 0;
        digitStates[2] = 0;

        digitRenderers[0] = transform.Find("digitHundreds").GetComponent<SpriteRenderer>();
        digitRenderers[1] = transform.Find("digitTens").GetComponent<SpriteRenderer>();
        digitRenderers[2] = transform.Find("digitUnits").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int currDist = Mathf.RoundToInt(Mathf.Abs(targetX - trackee.position.x)/1.12f);
        int[] newDigits = new int[3];
        newDigits[0] = currDist / 100;
        newDigits[1] = (currDist % 100) / 10;
        newDigits[2] = currDist % 10;

        for (int i=0; i<3; i++)
        {
            if (newDigits[i] != digitStates[i])
            {
                digitRenderers[i].sprite = numberSprites[newDigits[i]];
                digitStates[i] = newDigits[i];
            }
        }
    }
}
