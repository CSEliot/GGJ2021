using KiteLion.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public GameObject GameWinDisplay;
    public GameObject GameWinSnowmanPosition;
    public GameObject SnowmanHead;

    public GameObject Arrow;
    public GameObject LeftArrowSpawn;
    public GameObject RightArrowSpawn;
    public GameObject ForwardArrowSpawn;
    public GameObject BackArrowSpawn;

    public GameObject[] WinConditions;
    int _totalNeededWinConditions;
    int _currentWinConditions;

    public bool gameWin = false;
    bool _hasDoneGameWin = false;
    public float gameWinHeadScale = 0.1963986f;


    public int ArrowLifeTime = 2;

    float _sideInput;
    float _forwardInput;

    // Start is called before the first frame update
    void Start()
    {
        _totalNeededWinConditions = WinConditions.Length;
        PlayerPrefs.SetInt("CBUG_ON", 1);
        PlayerPrefs.Save();
        CBUG.Do("Sanity check Start");
    }

    // Update is called once per frame
    void Update()
    {
        //CBUG.Do("Sanity check Update");


        if (Input.GetButtonDown("Left")) {
            Destroy(GameObject.Instantiate(Arrow, LeftArrowSpawn.transform), ArrowLifeTime);
        }
        if (Input.GetButtonDown("Right"))
        {
            Destroy(GameObject.Instantiate(Arrow, RightArrowSpawn.transform), ArrowLifeTime);
        }
        if (Input.GetButtonDown("Up"))
        {
            Destroy(GameObject.Instantiate(Arrow, ForwardArrowSpawn.transform), ArrowLifeTime);
        }
        if (Input.GetButtonDown("Down"))
        {
            Destroy(GameObject.Instantiate(Arrow, BackArrowSpawn.transform), ArrowLifeTime);
        }

        _currentWinConditions = 0;
        foreach (GameObject snowball in GameObject.FindGameObjectsWithTag("Katamari"))
        {
            foreach (GameObject WinCondition in WinConditions)
            {
                if(snowball.name == WinCondition.name)
                {
                    if(snowball.GetComponent<FixedJoint>() != null)
                    {
                        foreach (GameObject otherWinCondition in WinConditions)
                        {
                            if( snowball.GetComponent<FixedJoint>().connectedBody.gameObject.name == otherWinCondition.name)
                            {
                                _currentWinConditions++;
                            }
                        }
                    }
                }

            }
        }
        
        if(gameWin == false && _currentWinConditions == _totalNeededWinConditions)
        {
            gameWin = true;
        }

        if(gameWin && _hasDoneGameWin == false)
        {
            _hasDoneGameWin = true;
            _DoGameWin();
        }
    }

    public void ReloadGame()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.UnloadSceneAsync(activeScene);
        SceneManager.LoadScene(activeScene, LoadSceneMode.Single);
    }

    void _DoGameWin()
    {
        GameWinDisplay.SetActive(true);
        SnowmanHead.GetComponent<Rigidbody>().isKinematic = true;
        //foreach (GameObject PhysicalObject in GameObject.FindGameObjectsWithTag("Katamari"))
        //{
        //    PhysicalObject.GetComponent<Rigidbody>().isKinematic = true;
        //}

        SnowmanHead.transform.position = GameWinSnowmanPosition.transform.position;
        SnowmanHead.transform.rotation = GameWinSnowmanPosition.transform.rotation;
        GameWinSnowmanPosition.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
