using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour
{
    [SerializeField] GameObject flag;
    [SerializeField] Transform RedGoal;
    [SerializeField] Transform BlueGoal;
    [SerializeField] Scrollbar flagscroll;
    // Start is called before the first frame update
    float dis_FlagToBlue;
    float dis_FlagToRed;
    float totalDis;
    float flagPos;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dis_FlagToBlue = Vector3.Distance(flag.transform.position, BlueGoal.position);

        dis_FlagToRed = Vector3.Distance(flag.transform.position, RedGoal.position);

        totalDis = dis_FlagToRed + dis_FlagToBlue;
        flagPos = dis_FlagToBlue / totalDis;
        flagscroll.value = flagPos;
        //Debug.Log(flagPos);
    }
}
