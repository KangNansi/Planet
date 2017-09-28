using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    public Planet p;
    protected Camera cam;
    protected Rigidbody rigidbody;
    protected Collider collider;

    public float speed = 10f;

    //Mouse
    public float mouseSensitivity = 2f;
    private float rotX = 0;

    private float pitch = 0;
    private float yaw = 0;

    private float jump = 0;
    
    public float eyeHeight;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        cam.transform.parent = transform;
        cam.transform.localPosition = new Vector3(0, eyeHeight, 0);
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 g = (transform.position - p.transform.position).normalized;
        //Player movement
        float hor = Input.GetAxis("Horizontal")*speed;
        float ver = Input.GetAxis("Vertical")*speed;
        yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        Quaternion yRot = Quaternion.AngleAxis(yaw, Vector3.up);
        Quaternion planetRotation = Quaternion.LookRotation(transform.forward, g);
        transform.rotation = planetRotation* yRot ;
        cam.transform.localEulerAngles = new Vector3(pitch, 0);
       // transform.rotation *= Quaternion.AngleAxis(rotX, transform.up);
        //transform.rotation *= Quaternion.FromToRotation(transform.up, g);
        //cam.transform.Rotate(Vector3.right, yrot += mouseY * mouseSensitivity);
        if (Input.GetButton("Jump"))
            jump = 100f;
        else
            jump -= speed/3f;
        if (jump < -10) jump = -10;

        Debug.Log(ver);
        rigidbody.velocity = transform.forward * ver * 5f + transform.right * hor * 5f + -g*(-jump+50f);
        //controller.Move(transform.up * -10f *Time.deltaTime);

        
       // Vector3 movement = (Input.GetButton("Run") ? runningSpeed : speed) * (transform.forward * ver + transform.right * hor);


    }

    private void OnPostRender()
    {
        Debug.DrawLine(transform.position, (transform.position - p.transform.position).normalized * -5f);
    }
}
