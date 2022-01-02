using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;

    void Start()
    {
        if (PlayerPrefs.HasKey("username")) //이름이 있으면 이름을 쓰고..
        {
            usernameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            usernameInput.text = "Player " + Random.Range(0, 10000).ToString("0000"); // 없으면 4자리 랜덤 숫자로 부여 
            OnUsernameInputValueChanged();
        }
    }
    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString("username", usernameInput.text); //Playerprefs는 게임을 종료했다가 다시 켜도 변화하지 않는 수치/파라미터 .지훈
    }
}
