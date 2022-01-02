using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    RectTransform pos;
    SpriteRenderer sr;
    public GameObject[] redTeam;
    public GameObject[] blueTeam;
    public GameObject[] redImages;
    public GameObject[] blueImages;
    //public int minimapRotation;

    // Start is called before the first frame update
    void Start()
    {
        //Transform trans;
        //pos = GetComponent<RectTransform>();
        //for (int i = 0; i < 4; i++)
        //{
        //    sr = images[i].GetComponent<SpriteRenderer>();
        //}
        //for (int i = 0; i < 4; i++)
        //{
        //    images[i] = transform.GetChild(i).gameObject;
        //}
        //for(int i=0;i<targets.Length;i++){
        //    Debug.Log(targets[i].transform.position);
        //}
        //trans = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        redTeam = GameObject.FindGameObjectsWithTag("TeamRed");
        blueTeam = GameObject.FindGameObjectsWithTag("TeamBlue");
        for (int i = 0; i < redTeam.Length; i++)
        {
            float x = redTeam[i].transform.position.x;
            float y = redTeam[i].transform.position.z;
            x = (x - 22) / 420 * 1378;
            y = (y - 134) / 504 * 1750;
            //pos.anchoredPosition = new Vector2(x, z);
            redImages[i].SetActive(true);
            redImages[i].GetComponent<Image>().rectTransform.localPosition = new Vector2(x, y);
            redImages[i].GetComponent<Image>().color = Color.red;
        }
        for (int i = 0; i < blueTeam.Length; i++)
        {
            float x = blueTeam[i].transform.position.x;
            float y = blueTeam[i].transform.position.z;
            x = (x - 22) / 420 * 1378;
            y = (y - 134) / 504 * 1750;
            //pos.anchoredPosition = new Vector2(x, z);
            blueImages[i].SetActive(true);
            blueImages[i].GetComponent<Image>().rectTransform.localPosition = new Vector2(x, y);
            blueImages[i].GetComponent<Image>().color = Color.blue;
        }
        //player 들 찾아내서 x,z값 받아오기.
        //os.anchoredPosition = new Vector2();
    }
}
