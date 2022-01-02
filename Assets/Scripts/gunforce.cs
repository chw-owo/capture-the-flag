using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunforce : MonoBehaviour
{
    public float mass = 0.2f;
    public float gravityparameter = 9.8f;
    public float movefrictionparameter;
    public float stopfrictionparameter;
    public LayerMask GroundMask;
    public bool isGrounded = false;

    public Vector3 velocity = new Vector3(0, 0, 0);
    public Vector3 accel = new Vector3(0, 0, 0);
    public Vector3 power = new Vector3(0, 0, 0);
    Vector3 currentpos;
    Vector3 springdistance;
    Vector3 impulse;
    bool springstate = false;

    // Start is called before the first frame update
    void Start()
    {
        gravityparameter = gravityparameter * 0.001f;
        currentpos = this.gameObject.transform.position;
    }

    public void Setimpulse(Vector3 inputimpulse)
    {
        impulse = inputimpulse;
    }

    public void Impulseprocessing()
    {
        if (impulse.magnitude != 0)
        {
            //Debug.Log("충격량 처리");
            var addvelocity = impulse / mass;
            velocity += addvelocity;
            impulse = new Vector3(0, 0, 0);
        }
    }
    Vector3 Friction()
    {
        GroundCheck();
        if (velocity.magnitude > 0 && isGrounded == true)
        {
            Vector3 frictionpower = -mass * movefrictionparameter * gravityparameter * velocity.normalized;
            Vector3 normalforce = mass * gravityparameter * new Vector3(0, 1, 0);
            Vector3 sumpower = frictionpower + normalforce;
            return sumpower;
        }
        else
        {
            if (isGrounded == true)
            {
                Vector3 normalforce = mass * gravityparameter * new Vector3(0, 1, 0);
                return normalforce;
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentpos = this.gameObject.transform.position;
        Vector3 gravitypower = new Vector3(0, -Mathf.Abs(mass * gravityparameter), 0);
        //Vector3 gravitypower = new Vector3(0, -29.4f, 0);
        //Debug.Log(gravitypower);
        Vector3 frictionpower = Friction();
        power = gravitypower + frictionpower;
        accel = power / mass;
        velocity = velocity + accel * Time.deltaTime;
        this.gameObject.transform.position = this.gameObject.transform.position + velocity * Time.deltaTime;
        //Debug.Log($"currentpos:{currentpos},power:{power},accel={accel},velocity={velocity}, gravitypower:{gravitypower},frictionpow:{frictionpower}");
        if (Input.GetButtonDown("Jump") && !springstate)
        {
            springstate = true;
        }
        Impulseprocessing();


    }

    private void GroundCheck()
    {
        if (this.gameObject.transform.position.y < 0.2f)
        {
            //Debug.Log("바닥면");
            isGrounded = true;
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0.2f, this.gameObject.transform.position.z);
            velocity = new Vector3(velocity.x, 0, velocity.z);
        }
        else if (this.gameObject.transform.position.y > 0.2f)
        {
            isGrounded = false;
        }
    }
}
