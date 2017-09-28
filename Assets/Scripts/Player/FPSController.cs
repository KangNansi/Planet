using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour {

    public float eyeHeight = 1.5f;
    public float mouseSensitivity = 1;
    public float speed = 3;
    public float runningSpeed = 6;
    public float jumpSpeed = 10;

    public LayerMask ObjectLayer;
    public GameObject hand;
    public GameObject arm;
    public Outline outline;
    private Renderer focused = null;

    float yrot = 0;
    float xrot = 0;

    protected Camera cam;
    protected CharacterController controller;

    //Controller state
    private bool isGrounded = true;
    private float vSpeed = 0;

	// Use this for initialization
	protected void Start () {
        //Create Camera
        cam = Camera.main;
        cam.transform.parent = transform;
        cam.transform.localPosition = new Vector3(0, eyeHeight, 0);
        if (arm)
        {
            arm.transform.parent = cam.transform;
            arm.transform.localPosition = new Vector3(0.33f, -0.28f, 0.41f);
        }
        outline = cam.GetComponent<Outline>();
        //Getting the controller
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	protected void FixedUpdate () {
        //Camera movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yrot += mouseX * mouseSensitivity;
        float cameraX = cam.transform.rotation.eulerAngles.x + mouseY * mouseSensitivity;
        Debug.Log(yrot);
        if (!(cameraX > 80 && cameraX < 270))
            xrot += mouseY * mouseSensitivity;
        transform.Rotate(Vector3.up, mouseX * mouseSensitivity);
        cam.transform.Rotate(Vector3.right, mouseY * mouseSensitivity);


        //Player movement
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 movement = (Input.GetButton("Run") ? runningSpeed : speed) * (transform.forward * ver + transform.right * hor);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isGrounded = false;
            vSpeed = jumpSpeed;
        }
        else if (isGrounded)
            vSpeed = 0;

        //Apply Gravity
        movement += (transform.up * vSpeed);
        //vSpeed = Mathf.Clamp(vSpeed-3,-10, 30000);

        //Interaction

        //Reset outlinez
        /*
        RaycastHit hit;
        Renderer rend = null;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 15, ObjectLayer))
        {
            Interactible inter = hit.collider.GetComponent<Interactible>();
            if (inter)
            {
                rend = inter.getRenderer();
                if (Input.GetButtonDown("Activate"))
                    inter.OnActivate(this.gameObject);
            }
        }
        outline.Focus(rend);*/

        Debug.Log(isGrounded);
        //State Calculation
        if(vSpeed < 0 && Physics.Raycast(transform.position, transform.up * -1, 3f))
        {
            isGrounded = true;
            vSpeed = 0;
        }
        else
        {
            isGrounded = false;
        }

        controller.Move(movement);
	}

}
