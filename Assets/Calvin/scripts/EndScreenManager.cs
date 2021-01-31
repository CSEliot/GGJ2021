using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndScreenManager : MonoBehaviour
{

    public List<Transform> SnowManPieces;
    GameObject centerHolder;
    public Transform snowManSlot;

    public TMP_Text meanThingsText;
    public List<string> meanThingsToSay;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if(SnowManPieces.Count > 0)
        {
            centerHolder = new GameObject();
            centerHolder.transform.position = FindCenterPoint(SnowManPieces);
            ParentList(SnowManPieces, centerHolder.transform);
            centerHolder.transform.SetParent(snowManSlot);
            centerHolder.transform.localPosition = new Vector3(0, 0, 0);
            centerHolder.transform.localScale = snowManSlot.localScale;
        }

        if(meanThingsText != null)
        {
            meanThingsText.text = meanThingsToSay[Random.Range(0, meanThingsToSay.Count)];
        }
    


    }
    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 FindCenterPoint(List<Transform> allPoints)
    {
        Transform[] pointArray = allPoints.ToArray();
        Vector3 centerPoint = new Vector3();

        for(int i = 0; i < pointArray.Length; i++)
        {
            centerPoint = centerPoint + pointArray[i].position;
        }

        centerPoint = centerPoint / pointArray.Length;
        return centerPoint;

    }


    public void ParentList(List<Transform> allPoints, Transform parentTransform)
    {
        foreach(Transform point in allPoints)
        {
            point.SetParent(parentTransform);
        }
    }


}
