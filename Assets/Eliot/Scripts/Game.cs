using KiteLion.Common;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    PhotonArenaManager _PM;
    public PhotonView MyPhotonView;

    public float MaxDistanceBeforeRespawn;
    public float SUPERMaxDistanceBeforeRespawn;
    public float MaxBelowDistanceBeforeRespawn;

    public GameObject GameWinDisplay;
    public GameObject GameWinSnowmanPosition;
    public GameObject SnowmanHead;

    public GameObject Arrow;
    public GameObject LeftArrowSpawn;
    public GameObject RightArrowSpawn;
    public GameObject ForwardArrowSpawn;
    public GameObject BackArrowSpawn;
    public GameObject GameRespawn;

    public GameObject[] WinConditions;
    int _totalNeededWinConditions;
    int _currentWinConditions;

    public bool gameWin = false;
    bool _hasDoneGameWin = false;
    public float gameWinHeadScale = 0.1963986f;

    public GameObject PreGameStuff;

    public Text UsernameText;

    public string Username;

    public GameObject Table;
    public TableController TableController;

    public enum _GameState
    {
        Pre,
        LoggingIn,
        Live,
        Post
    }

    public _GameState GameState = _GameState.Pre;

    public int ArrowLifeTime = 2;

    float _sideInput;
    float _forwardInput;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("CBUG_ON", 1);

        _PM = PhotonArenaManager.Instance;

        _totalNeededWinConditions = WinConditions.Length;
        PlayerPrefs.Save();
        CBUG.Do("Sanity check Start");

        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameState == _GameState.LoggingIn)
        {
            if(_PM.CurrentServerUserDepth == PhotonArenaManager.ServerDepthLevel.InRoom)
            {
                GameState = _GameState.Live;
            }
        }
        
        if(GameState == _GameState.Live)
        {
            if (Input.GetButtonDown("Left"))
            {
                MyPhotonView.RPC("SpawnLeftArrow", RpcTarget.All, Username);
            }
            if (Input.GetButtonDown("Right"))
            {
                MyPhotonView.RPC("SpawnRightArrow", RpcTarget.All, Username);
            }
            if (Input.GetButtonDown("Up"))
            {
                MyPhotonView.RPC("SpawnUpArrow", RpcTarget.All, Username);
            }
            if (Input.GetButtonDown("Down"))
            {
                MyPhotonView.RPC("SpawnBackArrow", RpcTarget.All, Username);
            }

            _currentWinConditions = 0;
            foreach (GameObject snowball in GameObject.FindGameObjectsWithTag("Katamari"))
            {
                foreach (GameObject WinCondition in WinConditions)
                {
                    if (snowball.name == WinCondition.name)
                    {
                        if (snowball.GetComponent<FixedJoint>() != null)
                        {
                            foreach (GameObject otherWinCondition in WinConditions)
                            {
                                if (snowball.GetComponent<FixedJoint>().connectedBody.gameObject.name == otherWinCondition.name)
                                {
                                    _currentWinConditions++;
                                }
                            }
                        }
                    }

                }
            }

            if (gameWin == false && _currentWinConditions == _totalNeededWinConditions)
            {
                gameWin = true;
            }

            if (gameWin && _hasDoneGameWin == false)
            {
                _hasDoneGameWin = true;
                _DoGameWin();
            }

            foreach(GameObject GameObj in GameObject.FindGameObjectsWithTag("Katamari"))
            {
                float objDistance = Vector3.Distance(GameObj.transform.position, GameRespawn.transform.position);
                if(GameObj.name.Contains("Mid"))
                {
                    CBUG.Log("Distance: " + objDistance);
                }
                if((GameObj.transform.position.y < MaxBelowDistanceBeforeRespawn && objDistance > MaxDistanceBeforeRespawn )
                    || objDistance > SUPERMaxDistanceBeforeRespawn)
                {
                    //CBUG.Log("Teleport device: " + GameObj.name);   
                    GameObj.GetComponent<Rigidbody>().isKinematic = true;
                    GameObj.transform.position = GameRespawn.transform.position;
                    GameObj.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
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
        GameState = _GameState.Post;
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

    public void GotUsername()
    {
        Username = UsernameText.text;
        GameState = _GameState.LoggingIn;
        DeactivePreGameStuff();
        LogIn();
    }

    public void DeactivePreGameStuff()
    {
        PreGameStuff.SetActive(false);
    }

    public void LogIn()
    {
        _PM.ConnectAndJoinRoom(Username, null);
    }

    [PunRPC]
    public void SpawnLeftArrow(string Username)
    {
        GameObject tempArrow = GameObject.Instantiate(Arrow, LeftArrowSpawn.transform);
        tempArrow.GetComponentInChildren<Text>().text = Username;
        Destroy(tempArrow, ArrowLifeTime);
        TableController.QueueLeftPush();
    }

    [PunRPC]
    public void SpawnRightArrow(string Username)
    {
        GameObject tempArrow = GameObject.Instantiate(Arrow, RightArrowSpawn.transform);
        tempArrow.GetComponentInChildren<Text>().text = Username;
        Destroy(tempArrow, ArrowLifeTime);
        TableController.QueueRightPush();
    }

    [PunRPC]
    public void SpawnUpArrow(string Username)
    {
        GameObject tempArrow = GameObject.Instantiate(Arrow, ForwardArrowSpawn.transform);
        tempArrow.GetComponentInChildren<Text>().text = Username;
        Destroy(tempArrow, ArrowLifeTime);
        TableController.QueueUpPush();
    }

    [PunRPC]
    public void SpawnBackArrow(string Username)
    {
        GameObject tempArrow = GameObject.Instantiate(Arrow, BackArrowSpawn.transform);
        tempArrow.GetComponentInChildren<Text>().text = Username;
        Destroy(tempArrow, ArrowLifeTime);
        TableController.QueueDownPush();
    }

}
