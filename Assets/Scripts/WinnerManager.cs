using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class WinnerManager : MonoBehaviour
{
    public GameObject BlueWin;
    public GameObject RedWin;
    public GameObject Replay;

    void Update()
    {
        if (Flag.Instance.red == 3)
        {
            RedWin.SetActive(true);
            Replay.SetActive(true);
        }
        else
        {
            RedWin.SetActive(false);
            Replay.SetActive(false);
        }
        if (Flag.Instance.blue == 3)
        {
            BlueWin.SetActive(true);
            Replay.SetActive(true);
        }
        else
        {
            BlueWin.SetActive(false);
            Replay.SetActive(false);
        }
    }

}
