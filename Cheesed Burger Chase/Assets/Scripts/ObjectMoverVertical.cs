using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectMoverVertical : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float moveDistance = 1;
    [SerializeField]
    private float startOffset = 0;
    private float startPos = 0.0f;
    private Rigidbody2D rb2D;
    bool reversed = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = Mathf.Abs(speed);
        startOffset = Mathf.Abs(startOffset);
        startPos = rb2D.transform.position.y;
        rb2D.transform.position = new Vector3(rb2D.transform.position.x, rb2D.transform.position.y + startOffset, rb2D.transform.position.z);
        rb2D.velocity = new Vector2(0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!reversed)
        {
            if (rb2D.transform.position.y >= startPos + moveDistance)
            {
                rb2D.velocity = new Vector2(0, -speed);
                reversed = true;
            }
        }
        else
        {
            if (rb2D.transform.position.y <= startPos)
            {
                rb2D.velocity = new Vector2(0, speed);
                reversed = false;
            }
        }
    }
}
