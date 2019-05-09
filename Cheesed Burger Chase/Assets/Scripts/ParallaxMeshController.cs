using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ParallaxMeshController : MonoBehaviour
{
    [SerializeField]
    private Transform followee;
    [SerializeField]
    private float followRate = 0.25f;
    private float prevPos;
    private MeshRenderer meshRenderer;
    private float quadWidth;
    private float offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = followee.transform.position.x;
        meshRenderer = GetComponent<MeshRenderer>();
        quadWidth = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followee.position.x, transform.position.y, transform.position.z);

        float distTravelled = prevPos - followee.transform.position.x;
        float offsetInc = (distTravelled / quadWidth) * (followRate - 1);
        offset += offsetInc;

        meshRenderer.material.mainTextureOffset = new Vector2(offset, 0);
        prevPos = followee.transform.position.x;
    }
}
