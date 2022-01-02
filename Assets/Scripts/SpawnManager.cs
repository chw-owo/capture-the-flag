using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    GameObject[] redTeamSpawns;
    GameObject[] blueTeamSpawns;

    void Awake()
    {
        instance = this;
        // Spawn 위치를 tag로 지정
        redTeamSpawns = GameObject.FindGameObjectsWithTag("RedSpawn");
        blueTeamSpawns = GameObject.FindGameObjectsWithTag("BlueSpawn");
    }

    // 팀에 따른 Spawn 위치 구분
    public Transform GetRandomRedSpawn()
    {
        //RedTeamSpawn으로 지정된 위치 중 랜덤으로 한 곳 return
        return redTeamSpawns[Random.Range(0, redTeamSpawns.Length)].transform;
    }

    public Transform GetRandomBlueSpawn()
    {
        //BlueTeamSpawn으로 지정된 위치 중 랜덤으로 한 곳 return
        return blueTeamSpawns[Random.Range(0, redTeamSpawns.Length)].transform;
    }

    public Transform GetTeamSpawn(int teamNumber)
    {
        // teamNumber가 0이면 red 팀으로, 그 외면 blue 팀으로
        return teamNumber == 0 ? GetRandomRedSpawn() : GetRandomBlueSpawn();
    }
}
