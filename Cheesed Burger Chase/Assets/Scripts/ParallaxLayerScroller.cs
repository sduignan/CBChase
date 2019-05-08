using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxLayerScroller : MonoBehaviour
{
    [SerializeField]
    private Transform followee;
    [SerializeField]
    private float followRate = 0.25f;
    private Vector3 prevPos;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = followee.transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        float spriteWidth = spriteRenderer.bounds.size.x;
        Debug.Log("GameObject: " + gameObject.name + "\tWidth: " + spriteWidth.ToString());
        spriteRenderer.material.SetTextureOffset("_MainTex", new Vector2(0.333f, 0));

    }

    // Update is called once per frame
    void Update()
    {
        float xOffset = followRate * (followee.position.x - prevPos.x);
        transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
        prevPos = followee.transform.position;

    }

    private void LateUpdate()
    {


    }
}
