using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{

    public float MaxButtonStrength = 6;

    public float TableResistance = 1;
    public int MaxPressesPerSecond = 10;

    public float MaxForwardRotation = 15;
    public float MaxSideRotation = 15;

    int _sideInput = 0;
    int _forwardInput = 0;
    float _rotForwardBack = 0;
    float _rotSideToSide = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _sideInput = 0;
        _sideInput += Input.GetButtonDown("Right") ? -1 : 0; // Z positive rotation rotates counter-clockwise???
        _sideInput += Input.GetButtonDown("Left") ? 1 : 0; // Z positive rotation rotates counter-clockwise???
        _forwardInput = 0;
        _forwardInput += Input.GetButtonDown("Up") ? 1 : 0;
        _forwardInput += Input.GetButtonDown("Down") ? -1 : 0;
    }

    private void FixedUpdate()
    {

        if (_rotForwardBack > 0)
            _rotForwardBack += -1 * TableResistance;
        if (_rotForwardBack < 0)
            _rotForwardBack += 1 * TableResistance;
        if (_rotSideToSide > 0)
            _rotSideToSide += -1 * TableResistance;
        if (_rotSideToSide < 0)
            _rotSideToSide += 1 * TableResistance;

        //GetComponent<Rigidbody>().isKinematic = true;
        CBUG.Do("PRINT");

        if (_forwardInput > 0 && _rotForwardBack > MaxForwardRotation)
            _forwardInput = 0;
        if (_forwardInput < 0 && _rotForwardBack < -1*MaxForwardRotation)
            _forwardInput = 0;
        if (_sideInput > 0 && _rotSideToSide > MaxSideRotation)
            _sideInput = 0;
        if (_sideInput < 0 && _rotSideToSide < -1*MaxSideRotation)
            _sideInput = 0;

        _rotForwardBack = _rotForwardBack + _forwardInput * MaxButtonStrength;
        _rotSideToSide = _rotSideToSide + _sideInput * MaxButtonStrength;


        GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(_rotForwardBack, 0, _rotSideToSide));

        //GetComponent<Rigidbody>().isKinematic = false;
    }
}
