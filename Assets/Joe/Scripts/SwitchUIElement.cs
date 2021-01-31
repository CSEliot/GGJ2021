using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUIElement : MonoBehaviour
{
    public GameObject canvasToSwitch;
    public GameObject myParentCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSwitch()
    {
        canvasToSwitch.SetActive(true);
        myParentCanvas.SetActive(false);
    }
}
