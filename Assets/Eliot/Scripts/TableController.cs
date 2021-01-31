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
    public float _rotForwardBack = 0;
    public float _rotSideToSide = 0;

    Queue<int> LeftPushQueue;
    Queue<int> RightPushQueue;
    Queue<int> UpPushQueue;
    Queue<int> DownPushQueue;

    PhotonArenaManager _PM;

    // Start is called before the first frame update
    void Start()
    {
        _PM = PhotonArenaManager.Instance;
        LeftPushQueue   = new Queue<int>();
        RightPushQueue  = new Queue<int>();
        UpPushQueue     = new Queue<int>(); 
        DownPushQueue   = new Queue<int>(); 
    }

    // Update is called once per frame
    void Update() 
    {
        _sideInput = 0;
        if (RightPushQueue.Count > 0)
        {
            RightPushQueue.Dequeue();
            _sideInput += -1; // Z positive rotation rotates counter-clockwise???
        }
        if (LeftPushQueue.Count > 0)
        {
            LeftPushQueue.Dequeue();
            _sideInput += 1; // Z positive rotation rotates counter-clockwise???
        }
        _forwardInput = 0;
        if (UpPushQueue.Count > 0)
        {
            UpPushQueue.Dequeue();
            _forwardInput += 1; // Z positive rotation rotates counter-clockwise???
        }
        if (DownPushQueue.Count > 0)
        {
            DownPushQueue.Dequeue();
            _forwardInput += -1; // Z positive rotation rotates counter-clockwise???
        }
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

        if (_PM.IsHost)
        {
            GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(_rotForwardBack, 0, _rotSideToSide));
        }

        //GetComponent<Rigidbody>().isKinematic = false;
    }

    public void QueueLeftPush()
    {
        LeftPushQueue.Enqueue(1);
    }
    public void QueueRightPush()
    {
        RightPushQueue.Enqueue(1);
    }
    public void QueueDownPush()
    {
        DownPushQueue.Enqueue(1);
    }
    public void QueueUpPush()
    {
        UpPushQueue.Enqueue(1);
    }
}
