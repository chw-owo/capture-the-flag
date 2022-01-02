using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    public Calculateforce a1;
    public Calculateforce a2;
    public float impulsemagnitude;
    public bool repulseable;
    
    public void Calculatemagneticforce(Calculateforce m_another1, Calculateforce m_another2)
    {
        Debug.Log("인력");
        Vector3 receiveimpulsedirection = (m_another2.transform.position - m_another1.transform.position).normalized;
        Vector3 sendimpulsedirection = -1f * receiveimpulsedirection;
        m_another2.Setimpulse(sendimpulsedirection * impulsemagnitude);
        m_another1.Setimpulse(receiveimpulsedirection * impulsemagnitude);
    }
    public void Calculaterepulsiveforce(Calculateforce m_another1, Calculateforce m_another2)
    {
        Debug.Log("충돌");
        Vector3 receiveimpulsedirection = (m_another2.transform.position - m_another1.transform.position).normalized;
        Vector3 sendimpulsedirection = -1f * receiveimpulsedirection;
        m_another2.Setimpulse(receiveimpulsedirection * impulsemagnitude);
        m_another1.Setimpulse(sendimpulsedirection * impulsemagnitude);
    }
    
    public GameObject BulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        repulseable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Calculatemagneticforce(a1, a2);
        }

        if (((a1.transform.position - a2.transform.position).magnitude < 1) && repulseable == true)
        {
            repulseable = false;
            Calculaterepulsiveforce(a1, a2);
        }

        if (((a1.transform.position - a2.transform.position).magnitude > 1))
        {
            repulseable = true;
        }


        if (Input.GetKeyDown(KeyCode.U))
        {
            var Randompos = new Vector3(Random.Range(-100f, 100f), Random.Range(5f, 50f), Random.Range(-100f, 100f));
            Debug.Log("Instantiate");
            GameObject bullet = Instantiate(BulletPrefab, Randompos, transform.rotation);
            var m_caldegree = bullet.GetComponent<Calculateforce>();
            m_caldegree.mass = Random.Range(1f, 10f);
            m_caldegree.gravityparameter = 9.8f;
            m_caldegree.collisionparameter = Random.Range(0, 0.8f);
            m_caldegree.movefrictionparameter = Random.Range(0, 0.8f);
            m_caldegree.stopfrictionparameter = Random.Range(0, 0.8f);

            //Impulse = -5 * cameraHolder.transform.forward.normalized;

            //spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
