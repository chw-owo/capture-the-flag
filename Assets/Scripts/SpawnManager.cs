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
        // Spawn ��ġ�� tag�� ����
        redTeamSpawns = GameObject.FindGameObjectsWithTag("RedSpawn");
        blueTeamSpawns = GameObject.FindGameObjectsWithTag("BlueSpawn");
    }

    // ���� ���� Spawn ��ġ ����
    public Transform GetRandomRedSpawn()
    {
        //RedTeamSpawn���� ������ ��ġ �� �������� �� �� return
        return redTeamSpawns[Random.Range(0, redTeamSpawns.Length)].transform;
    }

    public Transform GetRandomBlueSpawn()
    {
        //BlueTeamSpawn���� ������ ��ġ �� �������� �� �� return
        return blueTeamSpawns[Random.Range(0, redTeamSpawns.Length)].transform;
    }

    public Transform GetTeamSpawn(int teamNumber)
    {
        // teamNumber�� 0�̸� red ������, �� �ܸ� blue ������
        return teamNumber == 0 ? GetRandomRedSpawn() : GetRandomBlueSpawn();
    }
}
