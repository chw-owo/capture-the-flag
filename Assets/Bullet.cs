using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SingleShotGun
{
    public GameObject effect;
    // Start is called before the first frame update
    //float damage;
    //public Team attacker_team;
    public string myteam;
    public float r_damage;
    void Start()
    {
        //damage = ((GunInfo)itemInfo).damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(r_damage);
        if (other.CompareTag("TeamRed") || other.CompareTag("TeamBlue"))
        {
            //if (attacker_team.team_ != other.GetComponent<PlayerController>().myTeam.team_)
            //{
            if (myteam != other.tag)
            {
                
                other.GetComponent<IDamageable>()?.TakeDamage(r_damage);
                Destroy(gameObject);
                GameObject go = Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(go, 3f);
            }
                //other.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            //}
           

        }
        else
        {
            Destroy(gameObject);
            GameObject go = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(go, 1.4f);
        }
            
        
    }
}
