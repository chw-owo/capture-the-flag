using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.IO;

//public class Team
//{
 //   public int team_;
//}

public class PlayerController : MonoBehaviourPunCallbacks,IDamageable
{
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject ui;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, smoothTime;
    [SerializeField] public GameObject cameraHolder;
    [SerializeField] Item[] items;
    [SerializeField] GameObject flagInteraction;
    GameObject controller;
    Scoreboard scoreboard;
    GameObject ScoreCanvas;
    Image CaptureRate;
    bool Isflagmine;
    int itemIndex;
    int previoustemIndex = -1;
    float timecheck;
    float verticalLookrotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    private int team;
    public Image teamColor;
    public GameObject form1;
    public GameObject form2;

    Rigidbody rb; //차후 직접 구현하면 기존 스크립트는 제거

    PhotonView PV;
    Flag flag;

    //public Team myTeam = new Team();
    
    //Color PlayerColor;
    const float maxHealth = 100f;
    float currentHealth = maxHealth;
    //[HideInInspector] public int team;

    //추가======================================================================
    [SerializeField] public Camera cam;
    [SerializeField] Calculateforce m_caldegree;
    bool canFire;
    Vector3 Impulse;
    public float maxvelocity = 10;
    public float inputacc = 300;

    //public GameObject bulletPrefab;
    public float spawnRateMin = 0.8f;
    public float spawnRateMax = 2f;

    private Transform firedirection;
    public float spawnRate;
    public float shootAfter;
    private float timeAfterSpawn;
    //==========================================================================
    PlayerManager playerManager;

    public enum flagState
    {
        FLAGISFREE = 0,
        FLAGISMINE,
        FLAGISNOTMINE
    }

    public flagState CurrenFlagtState;

    void Awake()
    {
        form1.SetActive(true);
        form2.SetActive(false);
        Isflagmine = false;
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    void Start()
    {
        

        if (PV.IsMine)
        {
            EquipItem(0);

        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
            
        }

        //flagInteraction = GameObject.Find("FlagInteraction").transform.gameObject;
        TeamColor(); 
        flagInteraction.gameObject.SetActive(false);
        scoreboard = Scoreboard.Instance;
        ScoreCanvas = scoreboard.transform.parent.gameObject;
        ScoreCanvas.SetActive(false);
        CaptureRate = flagInteraction.transform.GetComponentInChildren<Image>();
        CaptureRate.fillAmount = 0;

    //추가======================================================================
        timeAfterSpawn = 0;
        //spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        
    }
    /*
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
    */

    //==========================================================================
    
    void Update()
    {
        shootAfter += Time.deltaTime;
        Cursor.visible = false;
        if (!PV.IsMine)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (form1.activeSelf)
            {
                form1.SetActive(false);
                form2.SetActive(true);
            }
            else if (form2.activeSelf)
            {
                form1.SetActive(true);
                form2.SetActive(false);
            }
            
        }
        CooltimeChecking.CL.SetPlayer(this);
        //추가=======================================================================
        // timeAfterSpawn을 갱신
        //timeAfterSpawn += Time.deltaTime;
       
        //if (timeAfterSpawn >= spawnRate)
        //{
        //    canFire = true;
        //}

        //Impulseprocessing();
        //==========================================================================
        
        if (CurrenFlagtState == flagState.FLAGISMINE)
        {
            Flag.Instance.FlagMaster(GetComponent<PlayerController>());
            flagInteraction.gameObject.SetActive(false);
        }
        
        Look();
        Move();
        //Jump();
        OnScoreboard();
        //for (int i = 0; i < items.Length; i++)
        //{
        //    if (Input.GetKeyDown((i + 1).ToString()))
        //    {
        //        EquipItem(i);
        //        break;
        //    }
        //}
        spawnRate = items[itemIndex].GetComponent<SingleShotGun>().itemInfo.cooltime;
        //추가======================================================================
        if (Input.GetMouseButtonDown(0))
        {
            
            if (spawnRate <= shootAfter)
            {
                canFire = true;
                shootAfter = 0;
            }
            if (canFire == true)
            {
                items[itemIndex].Use();
                canFire = false;
            }
        }
        //==========================================================================

        if (transform.position.y < -10f) // 맵 이탈시 죽음 처리/ 아마 우리는 wall 통과 버그시 죽는 용도로 쓰일 듯. 콜라이더 구현되면 벽너머로 deadzone을 만들어서 구성해도 됨
        {
            Debug.Log("Die");
            if(CurrenFlagtState == flagState.FLAGISMINE)
            {
                Flag.Instance.GetScore(3);
                CurrenFlagtState = flagState.FLAGISFREE;
            }
            Die();
        }
    }

    void OnScoreboard()
    {
        
        if (Input.GetKey(KeyCode.Tab))
        {
            ScoreCanvas.SetActive(true);
        }
        else
        {
            ScoreCanvas.SetActive(false);
        }
    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        Vector3 a = new Vector3(0, 0, 0);
        a += inputacc * cameraHolder.transform.forward.normalized * Input.GetAxisRaw("Vertical");
        a += inputacc * cameraHolder.transform.right.normalized * Input.GetAxisRaw("Horizontal");
        
        if (a.magnitude != 0)
        {
            m_caldegree.Setinputforce(a);
            //Debug.Log(a);
        }
        else
        {
            return;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_caldegree)
        {
            //if (m_caldegree.velocity.magnitude < 4f)
            //{
                var addvelocity = 12 * cameraHolder.transform.up.normalized / m_caldegree.mass;
                m_caldegree.velocity += addvelocity;
            //}
        }
    }

    public void EquipItem(int _index)
    {
        if (_index == previoustemIndex)
            return;

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);
        if(previoustemIndex != -1)
        {
            items[previoustemIndex].itemGameObject.SetActive(false);
        }
        previoustemIndex = itemIndex;
    }
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookrotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookrotation = Mathf.Clamp(verticalLookrotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookrotation;
    }
    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            return;
        }
        //rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    
    public void TakeDamage(float damage) //쏜 사람의 컴퓨터에서 작동
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage) //모두의 컴퓨터에서 연산되지만 if문으로 하여금 피해자의 컴퓨터에서만 작동하도록 함
    {
        if (!PV.IsMine)
            return;
        
        currentHealth -= damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;
        

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    
    void TeamColor()
    {
        teamColor.color = GetComponent<MeshRenderer>().material.color;
    }


    void Die()
    {
        if (CurrenFlagtState == flagState.FLAGISMINE)
        {
            Flag.Instance.GetScore(3);
            CurrenFlagtState = flagState.FLAGISFREE;
        }
        Debug.Log("Die");
        playerManager.Die();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (PV.IsMine)
        {
            if (other.CompareTag("flag"))
            {
                if (CurrenFlagtState == flagState.FLAGISFREE)
                {
                    flagInteraction.gameObject.SetActive(true);
                    flagInteraction.transform.position = flagInteraction.transform.parent.localPosition;
                    flagInteraction.transform.LookAt(transform.position);
                    flagInteraction.transform.Rotate(Vector3.up * 180);
                    if (Input.GetKey(KeyCode.Q))
                    {
                        timecheck += Time.deltaTime;

                        if (CaptureRate.fillAmount >= 1)
                        {
                            timecheck = 0;
                            CurrenFlagtState = flagState.FLAGISMINE;


                        }
                    }
                    else
                    {
                        timecheck = 0;

                    }
                    CaptureRate.fillAmount = timecheck ;
                }
            }
            else
            {
                flagInteraction.gameObject.SetActive(false);
                //Vector3 a = new Vector3(0, 0, 0);
                //a += inputacc * cameraHolder.transform.forward.normalized * Input.GetAxisRaw("Vertical");
                //a += inputacc * cameraHolder.transform.right.normalized * Input.GetAxisRaw("Horizontal");
                //Debug.Log(a.magnitude);
                ////Debug.Log("Hey!");
                //if (a.magnitude != 0)
                //{
                //    m_caldegree.Setinputforce(a);
                //    //Debug.Log(a);
                //}
                //else
                //{
                //    return;
                //}
            }
            //flagInteraction.gameObject.SetActive(false);
        }

        }
        

    
}
