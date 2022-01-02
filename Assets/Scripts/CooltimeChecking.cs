using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooltimeChecking : MonoBehaviour
{
    public Image Fill;
    PlayerController Player;
    float spawnRate;
    float shootAfter;
    public static CooltimeChecking CL;
    void Awake()
    {
        if(CL == null)
        {
            CL = this;
        }    
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            spawnRate = Player.GetComponent<PlayerController>().spawnRate;
            shootAfter = Player.GetComponent<PlayerController>().shootAfter;
            Fill = GetComponent<Image>();
            if (shootAfter > 0)
            {
                //fill.fillAmount = spawnRate - (shootAfter / spawnRate);
                Fill.fillAmount = 1 - (shootAfter / spawnRate);
            }
        }
    }

    public void SetPlayer(PlayerController _player)
    {
        Player = _player;
    }
}
