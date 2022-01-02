using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] balls;
    public GameObject[] spawnpoints;
    float timecheck;
    int[] fakebool = {0,0,0,0};

    public static ItemManager IM;

    void Awake()
    {
        if(IM == null)
        {
            IM = this;
        }
        return;
    }
    void Start()
    {
        
        for (int i = 0; i < 4; i++)
        {
            fakebool[i] = 0;
            Instantiate(balls[i], spawnpoints[i].transform.position + new Vector3(0,3f,0), Quaternion.identity);
        }    
    }

    void Update()
    {
        for (int i = 0; i < fakebool.Length; i++)
        {
            if(fakebool[i] == 1)
            {
                timecheck += Time.deltaTime;
                if (timecheck > 10f)
                {
                    Instantiate(balls[i], spawnpoints[i].transform.position +new Vector3(0, 3f, 0), Quaternion.identity);
                    timecheck = 0;
                    fakebool[i] = 0;
                }
            }
        }
        
    }

    public void Respawn(int i)
    {
        fakebool[i] = 1;
    }


}
