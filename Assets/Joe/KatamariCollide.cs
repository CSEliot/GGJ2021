using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KatamariCollide : MonoBehaviour
{

    public Rigidbody rigidbody;
    bool hasJoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        hasJoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When a collision is happening, affix the two objects together if they are katamari
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null && !hasJoint && collision.gameObject.tag == "Katamari")
        {
            gameObject.AddComponent<FixedJoint>();
            gameObject.GetComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            hasJoint = true;
        }
    }
}
