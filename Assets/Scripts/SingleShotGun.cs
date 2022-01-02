using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Gun
{
    public Camera cam;
    public GameObject cameraHolder;
    public Calculateforce m_caldegree;
    public float s_damage;
    Vector3 Impulse;
    
    //public GameObject Player;
    PhotonView PV;

    void Awake()
    {
        
        PV = GetComponent<PhotonView>();

    }

    public override void Use()
    {
        Impulseprocessing();
        Shoot();
    }
    void Start()
    {
        s_damage = ((GunInfo)itemInfo).damage1;
        
    }
    public void Impulseprocessing()
    {
        if (Impulse.magnitude != 0)
        {
            //Debug.Log("충격량 처리");
            var addvelocity = Impulse / m_caldegree.mass;
            m_caldegree.velocity += addvelocity;
            Impulse = new Vector3(0, 0, 0);
        }
    }

    void Shoot() //총알 발사 부분, 1차는 레이캐스트로 구현하되 물리적용하면서 포물선으로 변경 ㅡ> 카메라를 자유롭게 할지? 결정해야 할 수도
    {
        //Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        //ray.origin = cam.transform.position;
        //if(Physics.Raycast(ray,out RaycastHit hit))
        //{
            PV.RPC("RPC_Shoot", RpcTarget.All);
            //    PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
            //    //PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal, hit.collider.transform);

            //    if (hit.collider.transform.root.gameObject.tag == "Player")
            //    {
            
            //var targetTeam = hit.collider.transform.root.gameObject.tag;
            //Debug.Log("Shoot from");
            //Debug.Log(myTeam);
            //Debug.Log("Shoot to");
            //Debug.Log(targetTeam);

            //if (myTeam != targetTeam)
            //{   
            //    hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            //}
            //    }


            //}
            //PV.RPC("RPC_Shoot", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Shoot()
    {
        //Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        //if (colliders.Length != 0) {
        //    GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
        //    //GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal,Vector3.up) * bulletImpactPrefab.transform.rotation);
        //    Destroy(bulletImpactObj, 10f);
        //bulletImpactObj.transform.SetParent(colliders[0].transform);
        //}
        var myTeam1 = this.transform.root.transform.root.transform.root.gameObject.tag;
        GameObject go = Instantiate(NonraybulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
        go.GetComponent<Bullet>().myteam = myTeam1;
        go.GetComponent<Bullet>().r_damage = s_damage;
        go.GetComponent<gunforce>().Setimpulse(cameraHolder.transform.forward.normalized * 5f);
        Impulse = -5 * cameraHolder.transform.forward.normalized;
        //go.GetComponent<Rigidbody>().AddForce(transform.forward * 300f);
        Destroy(go, 10f);
        
    }
}
