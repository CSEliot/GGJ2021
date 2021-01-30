using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
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
        CBUG.Do("Sanity check Update");

    }
}
