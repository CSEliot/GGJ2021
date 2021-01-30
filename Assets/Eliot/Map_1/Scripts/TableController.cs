using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{

    public float MaxButtonStrength = 6;
    public int MaxPressesPerSecond = 10;

    public float MaxForwardRotation = 15;
    public float MaxSideRotation = 15;

    float _sideInput = 0;
    float _forwardInput = 0;
    float _rotForwardBack = 0;
    float _rotSideToSide = 0;

    int _rightButtonPressesPerSecond;
    int _leftButtonPressesPerSecond;
    int _forwardButtonPressesPerSecond;
    int _backButtonPressesPerSecond;

    float _rightButtonPressesPerSecondRatio;
    float _leftButtonPressesPerSecondRatio;
    float _forwardButtonPressesPerSecondRatio;
    float _backButtonPressesPerSecondRatio;

    int[] _rightButtonPressesPerSecondOver10 = new int[10];
    int[] _leftButtonPressesPerSecondOver10 = new int[10];
    int[] _forwardButtonPressesPerSecondOver10 = new int[10];
    int[] _backButtonPressesPerSecondOver10 = new int[10];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _sideInput = Input.GetAxis("Horizontal")*-1; // Z positive rotation rotates counter-clockwise???
        _forwardInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {

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
