using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBFollower : MonoBehaviour
{
    [SerializeField]
    private Transform followee;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(followee.position.x, followee.position.y, transform.position.z);
    }
}
