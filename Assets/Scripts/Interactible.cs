using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour {

    private bool focused = false;
    private float focusTime = 0;

    private Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public Renderer getRenderer()
    {
        return rend;
    }

    public virtual void OnActivate(GameObject activator)
    {
        Debug.Log("lol");
    }

}
