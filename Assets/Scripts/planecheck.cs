using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planecheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }


    void maketext()
    {
        Vector3 v3Direction = Vector3.forward;
        Quaternion downRotation = Quaternion.Euler(0, -1 * Camera.main.transform.rotation.eulerAngles.y, 0);
        Vector3 v3RotatedDirection = downRotation * v3Direction;
        Vector3 targetposition = Camera.main.transform.position + v3RotatedDirection;

    }
    // Update is called once per frame
    void Update()
    {

    }
}
