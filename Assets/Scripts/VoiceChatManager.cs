using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using System;
using Photon.Pun;

public class VoiceChatManager : MonoBehaviourPunCallbacks
{
    string appID = "4f9f814e73a5404c92d6249d71455ae8";
    
    public static VoiceChatManager Instance;

    IRtcEngine rtcEngine;
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() //아고라 엔진 key입력
    {
        rtcEngine = IRtcEngine.GetEngine(appID);

        rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccess;
        rtcEngine.OnLeaveChannel += OnLeaveChannel;
        rtcEngine.OnError += OnError;
    }

    void OnError(int error, string msg)
    {
        
    }

    void OnLeaveChannel(RtcStats stats)
    {
       
    }

    void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {

    }

    public override void OnJoinedLobby()
    {
        rtcEngine.JoinChannel(PhotonNetwork.CurrentRoom.Name);
    }
    public override void OnLeftLobby()
    {
        rtcEngine.LeaveChannel();
    }

    void OnDestroy()
    {
        IRtcEngine.Destroy();
    }
}
