using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleProjectile : MonoBehaviour
{
    public bool activate = false;
    private bool activated = false;
    public string activateID = "wakeUp";
    public bool attack = false;
    public string attackID = "attack";
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
        if(activate == true && activated == false)
        {
            SizeTickRoutine = SizeTicker(tentacle, sizeIncrementSeconds, sizeIncrement, targetScale);
            StartCoroutine(SizeTickRoutine);
            anim.SetTrigger(activateID);
        }

        if(attack == true)
        {
            Attack();
        }
    }


    void Attack()
    {
        if(attack == true)
        {
            anim.SetTrigger(attackID);
            attack = false;
        }
    }


    IEnumerator SizeTicker(GameObject target, float time, float sizeIncrease, Vector3 scaleGoal)
    {
        if(waitingToGrow == false)
        {
            waitingToGrow = true;

            yield return new WaitForSeconds(time);
            Vector3 oldScale = target.transform.localScale;
            Vector3 newScale = new Vector3(oldScale.x + sizeIncrease, oldScale.y + sizeIncrease, oldScale.z + sizeIncrease);

            if(newScale.x > scaleGoal.x)
            {
                target.transform.localScale = scaleGoal;
                activated = true;
            }
            else
            {
                target.transform.localScale = newScale;
            }


            waitingToGrow = false;
        }
        
    }

}
