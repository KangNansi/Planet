using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Outline : MonoBehaviour {

    public Material mat;
    public List<Renderer> objects = new List<Renderer>();

    private RenderTexture rendText;
    private RenderTargetIdentifier rtId;
    private CommandBuffer buffer;

	// Use this for initialization
	void Start () {
        rendText = new RenderTexture(Screen.width, Screen.height, 0);
        rtId = new RenderTargetIdentifier(rendText);

        buffer = new CommandBuffer();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
        RenderTexture.active = rendText;
        Graphics.ExecuteCommandBuffer(buffer);
        RenderTexture.active = null;

        mat.SetTexture("_OccludeMap", rendText);
        Graphics.Blit(source, destination, mat, 2);

    }

    private void setCommandBuffer()
    {
        buffer.Clear();
        buffer.ClearRenderTarget(true, true, Color.black, 0);
        buffer.SetRenderTarget(rtId);
        for(int i=0; i<objects.Count; i++)
            buffer.DrawRenderer(objects[i], mat, 0, 0);
    }

    public void AddRenderer(Renderer rend)
    {
        objects.Add(rend);
        setCommandBuffer();
    }

    public void RemoveRenderer(Renderer rend)
    {
        objects.Remove(rend);
        setCommandBuffer();
    }

    public void Focus(Renderer rend)
    {
        if (objects.Contains(rend))
            return;
        if (rend == null)
        {
            if(objects.Count > 0)
            {
                objects.Clear();
                setCommandBuffer();
            }
        }
        else
        {
            AddRenderer(rend);
        }
    }
}
