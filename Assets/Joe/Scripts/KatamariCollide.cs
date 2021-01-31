using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KatamariCollide : MonoBehaviour
{
    public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When a collision is happening, affix the two objects together if they are katamari tagged
    void OnCollisionEnter(Collision collision)
    {
        bool addComp = true;

        foreach (FixedJoint joint in gameObject.GetComponents<FixedJoint>())
        {
            if (joint.connectedBody == collision.gameObject.GetComponent<Rigidbody>()) { addComp = false; break; }
        }

        if (collision.gameObject.GetComponent<Rigidbody>() != null && addComp && collision.gameObject.tag == "Katamari")
        {
            gameObject.AddComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();

            if (!collision.gameObject.name.Contains("snow"))
            {
                if (collision.gameObject.GetComponent<MeshCollider>())
                {
                    Debug.Log("triggered");
                    collision.gameObject.GetComponent<MeshCollider>().isTrigger = true;
                }
            }
        }
    }
}
