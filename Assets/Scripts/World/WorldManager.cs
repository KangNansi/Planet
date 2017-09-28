using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    SkySphere sky;
	// Use this for initialization
	void Start () {
        GameObject s = new GameObject();
        sky = s.AddComponent<SkySphere>();        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
