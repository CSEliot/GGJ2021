using KiteLion.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject LeftArrowSpawn;
    public GameObject RightArrowSpawn;
    public GameObject ForwardArrowSpawn;
    public GameObject BackArrowSpawn;

    public int ArrowLifeTime = 2;

    float _sideInput;
    float _forwardInput;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("CBUG_ON", 1);
        PlayerPrefs.Save();
        CBUG.Do("Sanity check Start");
    }

    // Update is called once per frame
    void Update()
    {
        //CBUG.Do("Sanity check Update");


        if (Input.GetButtonDown("Left")) {
            Destroy(GameObject.Instantiate(Arrow, LeftArrowSpawn.transform), ArrowLifeTime);
        }
        if (Input.GetButtonDown("Right"))
        {
            Destroy(GameObject.Instantiate(Arrow, RightArrowSpawn.transform), ArrowLifeTime);
        }
        if (Input.GetButtonDown("Up"))
        {
            Destroy(GameObject.Instantiate(Arrow, ForwardArrowSpawn.transform), ArrowLifeTime);
        }
        if (Input.GetButtonDown("Down"))
        {
            Destroy(GameObject.Instantiate(Arrow, BackArrowSpawn.transform), ArrowLifeTime);
        }
    }
}
