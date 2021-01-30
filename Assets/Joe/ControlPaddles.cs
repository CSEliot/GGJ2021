using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlPaddles : MonoBehaviour
{
    [SerializeField] private GameObject leftPaddleRot, rightPaddleRot;
    [SerializeField] private float rotationSpeed = 360;
    [SerializeField] private string leftPaddleInput = "LeftPaddleRotate", rightPaddleInput = "RightPaddleRotate";
    [SerializeField] private float degreesToRotate = 90.0f;
    private bool isLeftPaddleRotating, isRightPaddleRotating;
    private bool leftRotateOut, rightRotateOut;
    private Quaternion rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        isLeftPaddleRotating = false;
        isRightPaddleRotating = false;

        leftPaddleRot = gameObject.transform.Find("LeftPaddleAxis").gameObject;
        rightPaddleRot = gameObject.transform.Find("RightPaddleAxis").gameObject;

        leftRotateOut = true;
        rightRotateOut = true;
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(leftPaddleInput) && !isLeftPaddleRotating)
        {
            isLeftPaddleRotating = true;
        }
        if (Input.GetButtonDown(rightPaddleInput) && !isRightPaddleRotating)
        {
            isRightPaddleRotating = true;
        }

        if (isLeftPaddleRotating)
        {
            if (leftRotateOut && leftPaddleRot.transform.eulerAngles.z >= degreesToRotate) { leftRotateOut = false; }
            else if (!leftRotateOut && (leftPaddleRot.transform.eulerAngles.z <= 0.0f || (leftPaddleRot.transform.eulerAngles.z > 100.0f && leftPaddleRot.transform.eulerAngles.z <= 360.0f)))
            {
                leftRotateOut = true;
                isLeftPaddleRotating = false;
            }
            else
            {
                if (leftRotateOut)
                {
                    rotation = Quaternion.Euler(0, 180, degreesToRotate);
                    leftPaddleRot.transform.rotation = Quaternion.RotateTowards(leftPaddleRot.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                    leftPaddleRot.transform.rotation = Quaternion.RotateTowards(leftPaddleRot.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
        if (isRightPaddleRotating)
        {

            if (rightRotateOut && rightPaddleRot.transform.eulerAngles.z >= degreesToRotate) { rightRotateOut = false; }
            else if (!rightRotateOut && (rightPaddleRot.transform.eulerAngles.z <= 0.0f || (rightPaddleRot.transform.eulerAngles.z > 100.0f && rightPaddleRot.transform.eulerAngles.z <= 360.0f)))
            {
                rightRotateOut = true;
                isRightPaddleRotating = false;
            }
            else
            {
                if (rightRotateOut) {
                    rotation = Quaternion.Euler(0, 0, degreesToRotate);
                    rightPaddleRot.transform.rotation = Quaternion.RotateTowards(rightPaddleRot.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }
                else {
                    rotation = Quaternion.Euler(0, 0, 0);
                    rightPaddleRot.transform.rotation = Quaternion.RotateTowards(rightPaddleRot.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
