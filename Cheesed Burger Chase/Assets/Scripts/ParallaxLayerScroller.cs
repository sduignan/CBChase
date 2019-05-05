using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayerScroller : MonoBehaviour
{
    [SerializeField]
    private Transform followee;
    [SerializeField]
    private float followRate = 0.25f;
    private Vector3 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = followee.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        float xOffset = followRate * (followee.position.x - prevPos.x);
        transform.position = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
        prevPos = followee.transform.position;
    }
}
