using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootBombs : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float frequency = 5f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;

        if (timer >= frequency)
        {
            timer = 0f;

            GameObject bomb = Instantiate(projectile, gameObject.transform.position, Quaternion.identity) as GameObject;
            bomb.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        }
    }
}
