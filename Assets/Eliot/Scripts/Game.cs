using KiteLion.Common;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject GreenSphere;
    public GameObject Santa;
    public EndScreenManager EndScreenMan;
    public EndScreenManager EndScreenManFAILURE;

    public int ActivateSantaSeconds = 10;

    public bool canWin = true;
    
    PhotonArenaManager _PM;
    public PhotonView MyPhotonView;

    public float MaxDistanceBeforeRespawn;
    public float SUPERMaxDistanceBeforeRespawn;
    public float MaxBelowDistanceBeforeRespawn;

    public GameObject GameWinDisplay;
    public GameObject GameFAILDisplay;
    public GameObject GameWinSnowmanPosition;
    public GameObject SnowmanHead;

    public GameObject Arrow;
    public GameObject LeftArrowSpawn;
    public GameObject RightArrowSpawn;
    public GameObject ForwardArrowSpawn;
    public GameObject BackArrowSpawn;
    public GameObject GameRespawn;
    public int RandomRespawnRange;

    private List<GameObject> WinConditions;
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

    private int debugUnlockCount = 90;

    public GameObject[] NetworkSpawnList;

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
        WinConditions = new List<GameObject>();
        _totalNeededWinConditions = 2;
        PlayerPrefs.Save();
        CBUG.Do("Sanity check Start");
        PreGameStuff.SetActive(true);
        GameWinDisplay.SetActive(false);
        
        if(Application.isEditor == false)
        {
            canWin = true;
        }

        Tools.DelayFunction(_DoGameFail, 60 * 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("`"))
        {
            debugUnlockCount--;
        }

        //Cause Win 
        if(debugUnlockCount < 0 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(FindAndSetWinConditions() == true)
            {
                foreach (GameObject WinCon in WinConditions)
                {
                    WinCon.GetComponent<Rigidbody>().isKinematic = true;
                    WinCon.transform.position = Vector3.zero;
                    WinCon.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }


        if(GameState == _GameState.LoggingIn)
        {
            if(_PM.CurrentServerUserDepth == PhotonArenaManager.ServerDepthLevel.InRoom)
            {
                GameState = _GameState.Live;
                Tools.DelayFunction(ActivateSanta, ActivateSantaSeconds);
                if(_PM.IsHost && _PM.GetData("Setup") == null)
                {
                    _PM.SaveData("Setup", true);
                    SpawnWinCons();
                    CBUG.Do("SETUP ROOM");
                } else if (_PM.IsHost == false)
                {
                    ////CBUG.Do("Sync Table!");
                    //float xRot = 0;
                    //float zRot = 0;
                    //if(_PM.GetData("TableRotX") != null)
                    //{
                    //    xRot = (float) _PM.GetData("TableRotX");
                    //}
                    //if (_PM.GetData("TableRotZ") != null)
                    //{
                    //    zRot = (float)_PM.GetData("TableRotZ");
                    //}
                    //CBUG.Do("WHY x " + xRot + " Z " + zRot);
                    //Table.transform.rotation = Quaternion.Euler(xRot, 0f, zRot);
                    //TableController._rotForwardBack = xRot;
                    //TableController._rotSideToSide = zRot; --t
                    //if (_PM.GetData("TableRotX") == null || _PM.GetData("TableRotZ") == null)
                    //{
                    //    //CBUG.Do("WHY");
                    //    Table.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //}  --t
                }
            }
        }
        
        if(GameState == _GameState.Live)
        {
            //CBUG.Do("Owner: " + (Table.GetComponent<PhotonView>().AmOwner ? 1 : 0));
            if(_PM.IsHost && Table.GetComponent<PhotonView>().AmOwner == false)
            {
                Table.GetComponent<PhotonView>().TransferOwnership(_PM.GetLocalPlayerID());
            }
            if (_PM.CurrentServerUserDepth == PhotonArenaManager.ServerDepthLevel.InRoom && _PM.IsHost)
            {
                //_PM.SaveData("TableRotX", Table.transform.rotation.eulerAngles.x);--t
                //_PM.SaveData("TableRotZ", Table.transform.rotation.eulerAngles.z);--t
                //CBUG.Do("Saving rots x" + Table.transform.rotation.eulerAngles.x); 
                //CBUG.Do("Saving rots z" + Table.transform.rotation.eulerAngles.z);
            }

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

            //test if win
            _currentWinConditions = 0;
            foreach (GameObject snowball in GameObject.FindGameObjectsWithTag("Katamari"))
            {
                if(FindAndSetWinConditions() == false)
                {
                    break;
                }
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
                if(canWin && _PM.IsHost)
                {
                    gameWin = true;
                }
            }

            if (gameWin && _hasDoneGameWin == false)
            {
                _hasDoneGameWin = true;
                MyPhotonView.RPC("_DoGameWin", RpcTarget.All);
            }

            foreach(GameObject GameObj in GameObject.FindGameObjectsWithTag("Katamari"))
            {
                float objDistance = Vector3.Distance(GameObj.transform.position, GameRespawn.transform.position);
                //CBUG.Log("DIST: " + objDistance);
                if((GameObj.transform.position.y < MaxBelowDistanceBeforeRespawn && objDistance > MaxDistanceBeforeRespawn )
                    || objDistance > SUPERMaxDistanceBeforeRespawn)
                {
                    RespawnGameObj(GameObj);
                }
            }
        }
    }

    public void RespawnGameObj(GameObject GameObj)
    {
        //don't teleport ALL parts of a join, just the snowball.
        if(GameObj.GetComponent<FixedJoint>() != null && GameObj.name.Contains("snow") == false)
        {
            return;
        }
        //if katamari has parent don't TP the child.
        if(GameObj.transform.parent != null)
        {
            return;
        }

        //CBUG.Log("Teleport device: " + GameObj.name);
        GameObj.GetComponent<Rigidbody>().isKinematic = true;
        int randAdd = Random.Range(RandomRespawnRange/-2, RandomRespawnRange/2);
        Vector3 respawnPos = new Vector3(GameRespawn.transform.position.x + randAdd,
                                            GameRespawn.transform.position.y,
                                            GameRespawn.transform.position.z + randAdd);
        GameObj.transform.position = respawnPos;
        GameObj.GetComponent<Rigidbody>().isKinematic = false;
        GameObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        foreach (Rigidbody child in GameObj.transform.GetComponentsInChildren<Rigidbody>())
        {
            child.velocity = Vector3.zero;
        }
    }
    
    public void SpawnWinCons()
    {
        foreach (GameObject NetSpawn in NetworkSpawnList)
        {
            int randAdd = Random.Range(RandomRespawnRange / -2, RandomRespawnRange / 2);
            Vector3 spawnPos = new Vector3(GameRespawn.transform.position.x + randAdd,
                                                GameRespawn.transform.position.y,
                                                GameRespawn.transform.position.z + randAdd);
            _PM.SpawnObject(NetSpawn.name, spawnPos, Quaternion.identity);
        }
    }

    public void ReloadGame()
    {
        PhotonNetwork.Disconnect();
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.UnloadSceneAsync(activeScene);
        SceneManager.LoadScene(activeScene, LoadSceneMode.Single);
    }

    public void DoFAIL()
    {
        MyPhotonView.RPC("_DoGameWin", RpcTarget.All);
    }

    [PunRPC]
    public void _DoGameWin()
    {
        int i = 0;
        foreach (GameObject WinObj in WinConditions)
        {
            EndScreenMan.SnowManPieces.Add(WinObj);
            i++;
        }
        _PM.GetRoom().IsOpen = false;
        _PM.GetRoom().IsVisible = false;
        GameState = _GameState.Post;
        GameWinDisplay.SetActive(true);
        //SnowmanHead.GetComponent<Rigidbody>().isKinematic = true;
        //foreach (GameObject PhysicalObject in GameObject.FindGameObjectsWithTag("Katamari"))
        //{
        //    PhysicalObject.GetComponent<Rigidbody>().isKinematic = true;
        //}

        //SnowmanHead.transform.position = GameWinSnowmanPosition.transform.position;
        //SnowmanHead.transform.rotation = GameWinSnowmanPosition.transform.rotation;
        //GameWinSnowmanPosition.transform.rotation = Quaternion.Euler(0, 0, 0);
        Tools.DelayFunction(ReloadGame, 10);
    }

    [PunRPC]
    public void _DoGameFail()
    {
        int i = 0;
        foreach (GameObject WinObj in WinConditions)
        {
            EndScreenManFAILURE.SnowManPieces.Add(WinObj);
            i++;
        }
        _PM.GetRoom().IsOpen = false;
        _PM.GetRoom().IsVisible = false;
        GameState = _GameState.Post;
        GameFAILDisplay.SetActive(true);
        //SnowmanHead.GetComponent<Rigidbody>().isKinematic = true;
        //foreach (GameObject PhysicalObject in GameObject.FindGameObjectsWithTag("Katamari"))
        //{
        //    PhysicalObject.GetComponent<Rigidbody>().isKinematic = true;
        //}

        //SnowmanHead.transform.position = GameWinSnowmanPosition.transform.position;
        //SnowmanHead.transform.rotation = GameWinSnowmanPosition.transform.rotation;
        //GameWinSnowmanPosition.transform.rotation = Quaternion.Euler(0, 0, 0);
        Tools.DelayFunction(ReloadGame, 10);
    }


    public void ActivateSanta()
    {
        Santa.SetActive(true);
    }

    public void GotUsername()
    {
        Username = UsernameText.text;
        GameState = _GameState.LoggingIn;
        DeactivePreGameStuff();
        LogIn();
    }

    public bool FindAndSetWinConditions()
    {
        foreach (GameObject WinCon in GameObject.FindGameObjectsWithTag("Katamari"))
        {
            if (WinCon.name.Contains("snow"))
            {
                if(WinConditions.Contains(WinCon) == false)
                {
                    WinConditions.Add(WinCon);
                }
            }
        }
        if(WinConditions.Count == 3)
        {
            return true;
        }else
        {
            return false;
        }
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
        if(_PM.IsHost)
        {
            TableController.QueueLeftPush();
        }
    }

    [PunRPC]
    public void SpawnRightArrow(string Username)
    {
        GameObject tempArrow = GameObject.Instantiate(Arrow, RightArrowSpawn.transform);
        tempArrow.GetComponentInChildren<Text>().text = Username;
        Destroy(tempArrow, ArrowLifeTime);
        if (_PM.IsHost) {
            TableController.QueueRightPush();
        }
    }

    [PunRPC]
    public void SpawnUpArrow(string Username)
    {
        GameObject tempArrow = GameObject.Instantiate(Arrow, ForwardArrowSpawn.transform);
        tempArrow.GetComponentInChildren<Text>().text = Username;
        Destroy(tempArrow, ArrowLifeTime);
        if(_PM.IsHost)
        {
            TableController.QueueUpPush();
        }
    }

    [PunRPC]
    public void SpawnBackArrow(string Username)
    {
        GameObject tempArrow = GameObject.Instantiate(Arrow, BackArrowSpawn.transform);
        tempArrow.GetComponentInChildren<Text>().text = Username;
        Destroy(tempArrow, ArrowLifeTime);
        if(_PM.IsHost)
        {
            TableController.QueueDownPush();
        }
    }
}
