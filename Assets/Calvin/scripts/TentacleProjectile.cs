using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleProjectile : MonoBehaviour
{
    public bool activate = false;
    private bool activated = false;
    public bool attack = false;
    public GameObject tentacle;
    public Vector3 targetScale = new Vector3(1,1,1);

    public float sizeIncrementSeconds = .125f;
    private float sizeIncrement = .1f;
    bool waitingToGrow = false;
    IEnumerator SizeTickRoutine;



    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = tentacle.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GrowTentacle()
    {

    }

    /*
    IEnumerator SizeTicker(GameObject target, float time, )
    {
        if(waitingToGrow == false)
        {
            waitingToGrow = true;

            yield return new WaitForSeconds(time);
            Vector3 oldScale = target.transform.localScale;
            target.transform.localScale = new Vector3(oldScale.x + )

        }
        
    }
    */
}
