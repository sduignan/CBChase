using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TableController : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;
    [SerializeField]
    private GameObject tableLeftSide;
    [SerializeField]
    private GameObject tableCentre;
    [SerializeField]
    private GameObject tableRightSide;
    [SerializeField]
    private float tableWidth;

    public void ResizeTable()
    {
        float centreWidth = tableWidth * 1.12f - 0.56f;
        SpriteRenderer centreSpriteRenderer = tableCentre.GetComponent<SpriteRenderer>();
        centreSpriteRenderer.size = new Vector2(centreWidth, 0.84f);
        
        if (!boxCollider)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }
        boxCollider.size = new Vector2(centreWidth + 0.28f, 0.42f);
        boxCollider.offset = new Vector2(0, -0.21f);

        if (!rb2d)
        {
            rb2d = GetComponent<Rigidbody2D>();
            rb2d.bodyType = RigidbodyType2D.Static;
        }

        tableCentre.transform.position = transform.position;
        tableLeftSide.transform.position = new Vector3(transform.position.x - (0.5f * centreWidth + 0.14f), transform.position.y, transform.position.z);
        tableRightSide.transform.position = new Vector3(transform.position.x + (0.5f * centreWidth + 0.14f), transform.position.y, transform.position.z);
    }
}
