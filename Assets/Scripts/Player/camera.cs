using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform TR;
    public float sensitibity = 100.0f;
    float rotation_x = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        //float VCamera = Input.GetAxis("Mouse Y") * sensitibity * Time.deltaTime;
        float HCamera = Input.GetAxis("Mouse X") * sensitibity * Time.deltaTime;

        //rotation_x -= VCamera;
        //rotation_x = Mathf.Clamp(rotation_x, -10f, 90f);

        TR.Rotate(Vector3.up * HCamera);
    }
}
