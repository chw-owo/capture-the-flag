using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    private GameObject controller;

    private int team = 1;
    private int nextPlayerTeam = 2;
    public static PlayerManager instance;

    private void OnEnable()
    {
        if (PlayerManager.instance == null)
        {
            PlayerManager.instance = this;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (PV.IsMine)
        {
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
            if (controller == null)
            {
                CreateController();
            }
        }

    }
    void Update()
    {
        if (!PV.IsMine)
        {
            if (controller)
                Destroy(controller);
        }

    }
    // Update is called once per frame

    public void UpdateTeam()
    {
        if (nextPlayerTeam == 1)
        {
            nextPlayerTeam = 2;
        }
        else if (nextPlayerTeam == 2)
        {
            nextPlayerTeam = 1;
        }
        else
        {
            Debug.Log("nextPlayerTeam");
            Debug.Log(nextPlayerTeam);
        }


    }

    void CreateController()
    {
        if (team == 1)
        {
            //Debug.Log("Red");
            Transform spawn = SpawnManager.instance.GetTeamSpawn(0);
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerRed"), spawn.position, spawn.rotation, 0, new object[] { PV.ViewID });
        }
        if (team == 2)
        {
            //Debug.Log("Blue");
            Transform spawn = SpawnManager.instance.GetTeamSpawn(1);
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerControllerBlue"), spawn.position, spawn.rotation, 0, new object[] { PV.ViewID });
        }
    }

    public int Get_team()
    {
        return team;
    }

    public void Die()
    {
        if (controller.tag == "TeamRed")
        {
            team = 1;
        }
        else if (controller.tag == "TeamBlue")
        {
            team = 2;
        }

        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    [PunRPC]
    void RPC_GetTeam()
    {
        team = nextPlayerTeam;
        UpdateTeam();
        PV.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, team);
    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        team = whichTeam;
    }
}