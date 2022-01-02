using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class RePlayGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);   //1π¯¿Œµ¶Ω∫ æ¿¿ª ∑ŒµÂ
    }
}
