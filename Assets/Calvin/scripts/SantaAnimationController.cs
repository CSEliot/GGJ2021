using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaAnimationController : MonoBehaviour
{

    public enum SantaState { happy, angry, attack};
    public SantaState currentState;

    public List<SantaStateGroup> santaStateGroups;

    [System.Serializable]
    public class SantaStateGroup
    {
        public GameObject santaObject;
        public SantaState state;

        public SantaStateGroup(GameObject santaObj, SantaState santaSt)
        {
            santaObject = santaObj;
            state = santaSt;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(SantaStateGroup st in santaStateGroups)
        {
            if(st.state != currentState)
            {
                st.santaObject.SetActive(false);
            }
            else
            {
                st.santaObject.SetActive(true);
            }
        }
    }
}
