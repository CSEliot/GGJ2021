using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowManHeadAnimationController : MonoBehaviour
{
    [Range(0, 1)]
    public float anxiety = 0f;
    public string anxietyParamID = "anxietyLevel";
    Animator headAnim;
    // Start is called before the first frame update
    void Start()
    {
        headAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        headAnim.SetFloat(anxietyParamID, anxiety);
    }
}
