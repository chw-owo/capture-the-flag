using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Calculateforce : MonoBehaviour
{
    PhotonView PV;
    [SerializeField] GameObject cameraHolder;
    public float jumpForce;
    public bool capable = true;
    public float mass = 3f;
    public float gravityparameter = 9.8f;
    //public Calculateforce anotherplayer;
    public float collisionparameter;
    public float movefrictionparameter;
    public float stopfrictionparameter;
    public bool isGrounded;
    public float Grounddistance;
    public bool isPlane;
    public float Planedistance;
    public Vector3 savednormalvector;

    public Vector3 impulse = new Vector3(0, 0, 0); // 충격량 처리
    public Vector3 inputforce = new Vector3(0, 0, 0); // 사용자 입력을 통한 외력
    public Vector3 velocity = new Vector3(0, 0, 0); // 현재 속도
    public Vector3 remainvelocity = new Vector3(0, 0, 0); // 평면에서의 속도
    public Vector3 accel = new Vector3(0, 0, 0); // 현재 가속도
    public Vector3 netforce = new Vector3(0, 0, 0); // 현재 알짜힘
    public Vector3 fricforce = new Vector3(0, 0, 0); // 현재 마찰력
    public Vector3 normal_f = new Vector3(0, 0, 0); // 수직항력
    Vector3 currentpos;

    bool springstate = false;
    public bool msg1 = true;
    public bool msg2 = true;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        //playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

        currentpos = this.gameObject.transform.position;
    }

    public void Setimpulse(Vector3 inputimpulse)
    {
        impulse = inputimpulse;
    }

    //충격량 처리 함수
    public void Impulseprocessing()
    {
        if (impulse.magnitude != 0)
        {
            var addvelocity = impulse / mass;
            velocity += addvelocity;
            impulse = new Vector3(0, 0, 0);
        }
    }

    public void Setinputforce(Vector3 inputforce2)
    {
        inputforce = inputforce2;
        
    }

    public Vector3 Inputforceprocessing()
    {
        if (inputforce.magnitude != 0)
        {
            
            //inputforce = new Vector3(0, 0, 0);
            return inputforce;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    void Planecheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 500f))
        {
            var Planedistance = hit.distance;
            //Debug.Log($"F, distance:{Planedistance}");

            if (hit.distance <= 5f)
            {
                //Debug.Log($"F in 5meter:{Planedistance}");
                if (velocity.z >= 0f)
                {
                    velocity.z = 0f;
                }

            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, 500f))
        {
            var Planedistance = hit.distance;
            //Debug.Log($"F, distance:{Planedistance}");

            if (hit.distance <= 1f)
            {
                //Debug.Log($"F in 5meter:{Planedistance}");
                if (velocity.z <= 0f)
                {
                    velocity.z = 0f;
                }

            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 500f))
        {
            var Planedistance = hit.distance;
            //Debug.Log($"L, distance:{Planedistance}");

            if (hit.distance <= 1f)
            {
                //Debug.Log($"L in 5meter:{Planedistance}");
                if (velocity.x <= 0f)
                {
                    velocity.x = 0f;
                }

            }
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 500f))
        {
            var Planedistance = hit.distance;
            

            if (hit.distance <= 1f)
            {
                
                if (velocity.x >= 0f)
                {
                    velocity.x = 0f;
                }
            }
        }
    }

    void Groundcheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000f, Color.red);
        //Debug.Log(transform.TransformDirection(Vector3.down));
        float check = Vector3.Dot(transform.TransformDirection(Vector3.down), new Vector3(0, -1, 0));
        float a;
        //Debug.Log($"direction:{(-1f) * transform.TransformDirection(Vector3.down)},reverse direction:{transform.TransformDirection(Vector3.down)}");
        if (check < 0)
        {
            //Debug.Log("a=-1");
            a = -1f;
        }
        else
        {
            //Debug.Log("a=1");
            a = +1f;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 500f))
        {
            Grounddistance = hit.distance;
            //Debug.Log($"Ground, distance:{Grounddistance}");
            if (hit.distance < 2.85f)
            {
                //Collideprocessing();
                //Debug.Log($"Ground, distance:{Grounddistance}");
                isGrounded = true;

            }

            else if (hit.distance > 2.9f)
            {
                //Debug.Log($"Not Ground, distance:{Grounddistance}");
                isGrounded = false;
            }
        }
        else
        {
            //Debug.Log("Not hit");
            isGrounded = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //currentpos = this.gameObject.transform.position;

        currentpos = this.gameObject.transform.position;

        var gravityforce = Getgravityforce();
        var frictionforce = Getfrictionforce();
        fricforce = frictionforce;
        var normalforce = Getnormalforce();
        normal_f = normalforce;
        var frictiondirectionvector = Getfrictiondirectionvector();
        var dragforce = normalforce + gravityforce; // normalforce + gravityforce


        RaycastHit hit;
        Vector3 planecheck = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
        if (Physics.Raycast(planecheck, transform.TransformDirection(Vector3.down), out hit, 500f))
        {
            var planetag = hit.collider.gameObject.tag;
            

            if (planetag == "SlipZone")//(hit.collider.CompareTag("SlipZone"))
            {
                netforce = (gravityforce + normalforce) * 3f;
            }
            else
            {
                netforce = gravityforce + frictionforce + normalforce;
            }
        }

        if ((isGrounded == true) && (velocity.magnitude < 0.01f) && (netforce.magnitude < 0.1f))
        {
            netforce = new Vector3(0, 0, 0);
        }

        netforce = netforce + Removinginputnormalforce(Inputforceprocessing());
        inputforce = new Vector3(0, 0, 0);
        accel = netforce / mass;
        velocity = velocity + accel * Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Space)) { velocity.y = jumpForce; }
        if (Mathf.Abs(velocity.y) <= 2.0f) { velocity.y = 0; }
        if (Physics.Raycast(planecheck, transform.TransformDirection(Vector3.down), out hit, 500f))
        {
            var planetag = hit.collider.gameObject.tag;
            

            if (planetag == "flag")//(hit.collider.CompareTag("SlipZone"))
            {
                velocity.y = 0f;
            }

        }
        Planecheck();
        this.gameObject.transform.position = this.gameObject.transform.position + velocity * Time.deltaTime;

        Impulseprocessing();
        if (msg1 == true)
        {
            //Debug.Log(inputforce);
            //Debug.Log($"currentpos:{currentpos},netforce:{netforce},accel={accel},velocity={velocity}");
            //Debug.Log($"gravityforce:{gravityforce},frictionforce:{frictionforce}, normalforce:{normalforce},netforce:{netforce}");

            Debug.DrawRay(transform.position, gravityforce, Color.green);
            Debug.DrawRay(transform.position, frictionforce, Color.yellow);
            Debug.DrawRay(transform.position, normalforce, Color.black);
            Debug.DrawRay(transform.position, frictiondirectionvector, Color.red);
            Debug.DrawRay(transform.position, netforce, Color.cyan);
            Debug.DrawRay(transform.position, dragforce, Color.magenta);

        }
        if (Input.GetButtonDown("Jump") && !springstate)
        {
            springstate = true;
        }

        Groundcheck();
        //Planecheck();
        if (Input.GetKeyDown(KeyCode.L))
        {
            Removinginputnormalforce(velocity);
        }
    }

    float Getangle(Vector3 v1, Vector3 v2)
    {
        var vector1 = v1.normalized;
        var vector2 = v2.normalized;
        float dot = Vector3.Dot(vector1, vector2);
        float angle = Mathf.Acos(dot);
        return angle;
    }

    float Getcos(Vector3 v1, Vector3 v2)
    {
        var vector1 = v1.normalized;
        var vector2 = v2.normalized;
        float dot = Vector3.Dot(vector1, vector2);
        float angle = Mathf.Acos(dot);
        return Mathf.Cos(angle);
    }

    float Getsin(Vector3 v1, Vector3 v2)
    {
        var vector1 = v1.normalized;
        var vector2 = v2.normalized;
        float dot = Vector3.Dot(vector1, vector2);
        float angle = Mathf.Acos(dot);
        return Mathf.Sin(angle);
    }

    Vector3 Getnormalforce()
    {
        if (isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
                //Debug.Log("Hit:" + hit.collider.gameObject.name);
                var hitnormal = hit.normal;
                Vector3 normalforce = mass * gravityparameter * hitnormal * Getcos(hitnormal, new Vector3(0, 1, 0));
                //Debug.Log(normalforce.magnitude);
                return normalforce;
            }

            else
            {
                return new Vector3(0, 0, 0);
            }
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    Vector3 Getgravityforce()
    {
        Vector3 gravityforce = new Vector3(0, -Mathf.Abs(mass * gravityparameter), 0);
        return gravityforce;
    }

    Vector3 Getfrictionforce()
    {
        if (velocity.magnitude > 0.05f && isGrounded == true)
        {
            //Vector3 frictionpower = Getnormalforce().magnitude * movefrictionparameter * Getfrictiondirectionvector();
            Vector3 frictionforce = -1f * Getnormalforce().magnitude * movefrictionparameter * velocity.normalized;

            return frictionforce;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    Vector3 Getcrossvector(Vector3 v1, Vector3 v2)
    {
        Vector3 crossvector = Vector3.Cross(v1.normalized, v2.normalized);
        return crossvector.normalized;
    }

    Vector3 Getfrictiondirectionvector()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            var hitnormal = hit.normal;
            Vector3 normalforce = gravityparameter * hitnormal * Getcos(hitnormal, new Vector3(0, -1, 0));
            var gravitydirectionvector = new Vector3(0, -1, 0);
            var planenormalvector = hit.normal.normalized;
            var crossvector = Getcrossvector(gravitydirectionvector, planenormalvector);

            if (Mathf.Abs(Vector3.Dot(gravitydirectionvector.normalized, planenormalvector.normalized)) == 1)
            {
                crossvector = Getcrossvector(gravitydirectionvector, velocity);
            }
            Debug.DrawRay(transform.position, crossvector * 1000, Color.white);
            Vector3 frictiondirectionvector = (Quaternion.AngleAxis(-90, crossvector) * normalforce).normalized;

            if (Vector3.Dot(frictiondirectionvector, velocity) > 0)
            {
                frictiondirectionvector = -1 * frictiondirectionvector;
            }

            return frictiondirectionvector;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PV.IsMine)
        {
            return;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (!PV.IsMine)
        {
            return;
        }
        Collideprocessing();
        if (isGrounded == false)
        {

            isGrounded = true;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                if (savednormalvector.normalized == hit.normal.normalized)
                {
                    return;
                }
                //transform.up = hit.transform.up;
                //var tmprot = transform.rotation;
                else if (hit.normal != savednormalvector)
                {
                    Quaternion rot = hit.transform.rotation;
                    if (Vector3.Dot(transform.forward, rot * Vector3.forward) > 0)
                    {
                        transform.rotation = rot;
                    }
                    else
                    {
                        transform.rotation = rot;
                        transform.forward = -1f * transform.forward;
                    }
                    savednormalvector = hit.normal;
                    /*
                    if(Vector3.Dot(transform.forward, velocity)<0)
                    {
                        transform.forward = -1f * transform.forward;
                    }*/

                }

                if (hit.distance < 0.5f)
                {
                    Debug.Log("바닥면");
                    this.gameObject.transform.position = hit.point + hit.normal.normalized * 0.5f;
                }
            }
        }
        else if (isGrounded == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {

                if (hit.distance < 0.5f)
                {
                    //Debug.Log("바닥으로 들어감");
                    this.gameObject.transform.position = hit.point + hit.normal.normalized * 0.5f;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        capable = true;
        // Destroy everything that leaves the trigger

    }


    float Getplanedistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            return hit.distance;
        }
        else
        {
            return 99999f;
        }
    }


    float Getcollideforce()
    {
        var normalforcedirection = Getnormalforce().normalized;
        var reversenormalforce = (-1 * Getnormalforce()).normalized;
        var velocitynormalforcemagnitude = Vector3.Dot(normalforcedirection, velocity);
        return Mathf.Abs(velocitynormalforcemagnitude);
    }

    Vector3 Removinginputnormalforce(Vector3 v1)
    {
        var normalforcedirection = Getnormalforce().normalized;
        var reversenormalforce = (-1 * Getnormalforce()).normalized;
        var velocitynormalforcemagnitude = Vector3.Dot(normalforcedirection, v1);
        //var repulsiveforce = Mathf.Abs(velocitynormalforcemagnitude) * normalforcedirection * (1 + collisionparameter);
        Vector3 repulsiveforce;
        var processedforce = v1 - velocitynormalforcemagnitude * normalforcedirection;
        if (Mathf.Abs(velocitynormalforcemagnitude) < 0.1f)
        {
            repulsiveforce = Mathf.Abs(velocitynormalforcemagnitude) * normalforcedirection;
        }
        Debug.DrawRay(transform.position, processedforce, Color.red);
        //Debug.Log($"repulsiveforce:{repulsiveforce}");
        return processedforce;
        //velocity = velocity + Mathf.Abs(Vector3.Dot(normalforcedirection, velocity)) * normalforcedirection * (3 + collisionparameter);
        //Vector3.Dot(normalforce.normalized,velocity);

    }
    void Collideprocessing()
    {
        var normalforcedirection = Getnormalforce().normalized;
        var reversenormalforce = (-1 * Getnormalforce()).normalized;
        var velocitynormalforcemagnitude = Vector3.Dot(normalforcedirection, velocity);
        //var repulsiveforce = Mathf.Abs(Vector3.Dot(normalforcedirection, velocity)) * normalforcedirection * (1 + collisionparameter);
        var repulsiveforce = Mathf.Abs(velocitynormalforcemagnitude) * normalforcedirection * (1 + collisionparameter);
        var remainvelocity = velocity + Mathf.Abs(velocitynormalforcemagnitude) * normalforcedirection;
        if (Mathf.Abs(velocitynormalforcemagnitude) < 0.1f)
        {
            repulsiveforce = Mathf.Abs(velocitynormalforcemagnitude) * normalforcedirection;
        }
        //Debug.Log($"repulsiveforce:{repulsiveforce}");
        velocity = velocity + repulsiveforce;
        //velocity = velocity + Mathf.Abs(Vector3.Dot(normalforcedirection, velocity)) * normalforcedirection * (3 + collisionparameter);
        Debug.DrawRay(transform.position, repulsiveforce, Color.white);
        //Vector3.Dot(normalforce.normalized,velocity);

    }

}
