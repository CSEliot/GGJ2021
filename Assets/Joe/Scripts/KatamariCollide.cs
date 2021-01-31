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
        if(GetComponent<Rigidbody>() == null || rigidbody == null)
        {
            return;
        }   

        bool addComp = true;
        bool isSnowball = gameObject.name.Contains("snow");
        bool otherIsSnowball = collision.gameObject.name.Contains("snow");

        foreach (FixedJoint joint in gameObject.GetComponents<FixedJoint>())
        {
            if (joint.connectedBody == collision.gameObject.GetComponent<Rigidbody>()) { addComp = false; break; }
        }

        if (collision.gameObject.GetComponent<Rigidbody>() != null && addComp && collision.gameObject.tag == "Katamari")
        {
            //don't allow non-snow things to connect
            if(isSnowball == false && otherIsSnowball == false)
            {
                return;
            }

            if(isSnowball && otherIsSnowball && GetComponent<FixedJoint>() == null && collision.gameObject.GetComponent<FixedJoint>() == null)
            {
                gameObject.AddComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                Destroy(Instantiate(GameObject.FindGameObjectWithTag("Game").GetComponent<Game>().GreenSphere, collision.GetContact(0).point, Quaternion.identity), 3);
                gameObject.transform.SetParent(collision.gameObject.transform, true);
            }

            if(isSnowball == false && otherIsSnowball && GetComponent<FixedJoint>() == null)
            {
                gameObject.AddComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
                Destroy(Instantiate(GameObject.FindGameObjectWithTag("Game").GetComponent<Game>().GreenSphere, collision.GetContact(0).point, Quaternion.identity), 3);
                gameObject.transform.SetParent(collision.gameObject.transform, true);
            }

            if(rigidbody.gameObject.name.Contains("snow") == false)
            {
                if (rigidbody.gameObject.GetComponent<MeshCollider>() != null) {
                    //CBUG.Do("TRIG MESH");
                    rigidbody.gameObject.GetComponent<MeshCollider>().isTrigger = true;
                    rigidbody.useGravity = false;
                }
            }
        }
    }
}
