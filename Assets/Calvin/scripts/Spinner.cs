using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Vector3 spinTransfrom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.localEulerAngles = this.gameObject.transform.localEulerAngles + (spinTransfrom * Time.deltaTime);
    }
}
