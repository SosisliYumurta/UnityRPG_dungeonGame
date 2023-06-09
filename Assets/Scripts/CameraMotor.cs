using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookAt;
    public float boundX = 0.15f;
    public float boundY = 0.5f;




    // Start is called before the first frame update
    private void Start()
    {
        lookAt = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        // This is to check if we are inside bounds on the X axis
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x < lookAt.position.x)
                delta.x = deltaX - boundX;
            else
                delta.x = deltaX + boundX;
        }


        // This is to check if we are inside bounds on the Y axis
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY)
            if (transform.position.y < lookAt.position.y)
                delta.y = deltaY - boundY;
            else
                delta.y = deltaY + boundY;

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
