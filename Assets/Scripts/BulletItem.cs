using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    public int GunNum;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("TeamBlue") || other.CompareTag("TeamRed"))
        {
            other.GetComponent<PlayerController>().EquipItem(GunNum);
            Destroy(gameObject);
            ItemManager.IM.Respawn(GunNum-1);
        }
    }
}
