using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour {
    Camera cam;

    public float mouseSensitivity;
    public float speed;

    private float pitch = 0;
    private float yaw = 0;
    // Use this for initialization
    void Start () {
        //Create Camera
        cam = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        bool run = Input.GetButton("Run");
        float fspeed = (run) ? speed * 2f : speed;

        transform.Translate(ver * Vector3.forward * 10 * Time.deltaTime * fspeed);
        transform.Translate(hor * Vector3.right * 10 * Time.deltaTime * fspeed);
        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        //transform.Rotate(Vector3.right, mouseY * mouseSensitivity);
    }
}
