using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BombEvents : MonoBehaviour
{
    [SerializeField] private float power = 10f;
    [SerializeField] private float radius = 10f;
    [SerializeField] private float upforce = 1f;
    private float timeToDestroy = 10f;
    bool destroy = true;

    // Start is called before the first frame update
    void Start()
    {
        destroy = true;
        timeToDestroy = 10f;
        //Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy == true) { Destroy(gameObject, timeToDestroy); }
        //timeToDestroy = timeToDestroy - Time.deltaTime;

        //if (timeToDestroy <= 0f) { Destroy(gameObject); }
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody != null) { rigidbody.AddExplosionForce(power, gameObject.transform.position, radius, upforce, ForceMode.Impulse); }
        }

        Destroy(gameObject, 0f);
    }
}
