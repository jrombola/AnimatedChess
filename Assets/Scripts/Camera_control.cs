using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_control : MonoBehaviour
{
    public CharacterController Camera;
    float mouseSensitivity = 100f;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse ScrollWheel");


        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            Vector3 move = transform.forward * x;
            Camera.Move(move * 10);

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            Vector3 move = transform.forward * x;
            Camera.Move(move * 10);
        }
    }
}
