using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class World : MonoBehaviour {

    Terrain terrain;
    TerrainData terrainData;

    [System.Serializable]
    public class Noise
    {
        public float xOff, yOff;
        public float frequency;
        public float strength;
        public float offset;
        public float factor=1f;
    };
    [SerializeField]
    public List<Noise> noises = new List<Noise>();


    public float texHeight;
    public int treeDensity;
    public float waterLevel;

    // Use this for initialization
    void Start () {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Generate()
    {
        TerrainData terrainData = GetComponent<Terrain>().terrainData;
        float seed = Random.Range(-1000, 1000);
        float secondSeed = Random.Range(-1000, 1000);
        int size = terrainData.heightmapResolution;
        float[,] heights = new float[size, size];

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size;
                float y = j / (float)size;
                /*
                float global = Mathf.PerlinNoise(x, y);
                float r1 = Mathf.PerlinNoise(secondSeed + x * r1Scale, secondSeed + y * r1Scale);
                float third = Mathf.PerlinNoise(seed * 50f + x * thirdScale, seed * 50f + y * thirdScale);
                float high = Mathf.PerlinNoise(seed * 100f + x * highScale, seed * 100f + y * highScale);
                float veryHigh = Mathf.PerlinNoise(seed * 100f + x * vhighScale, seed * 100f + y * vhighScale);
                if (global < 0.5f) global = 0f;
                else global = (global - 0.5f) * 2;
                float variation = Random.Range(0, 1f);
                heights[i, j] = third*(global*0.9f + 0.09f*high + 0.01f * veryHigh + 0.01f * variation);*/
                float dc = Mathf.Max(Mathf.Abs(x - 0.5f), Mathf.Abs(y - 0.5f)) * 2f ;
                heights[i, j] = 0;
                foreach (Noise n in noises)
                    heights[i, j] += n.strength * (n.offset + (Mathf.PerlinNoise(n.xOff + x * n.frequency, n.yOff + y * n.frequency)-0.5f)*n.factor + 0.5f);
                if (dc > 0.7f)
                    heights[i, j] = 0;
                else
                    heights[i, j] *= (1 - dc*(1/0.7f));
                if (heights[i, j] < 0) heights[i, j] = 0;
            }
        terrainData.SetHeights(0, 0, heights);
        GetComponent<Terrain>().Flush();
        SetAlphamaps(terrainData);
        SetDetails(terrainData);
    }

    void SetAlphamaps(TerrainData terrainData)
    {
        Terrain terrain = GetComponent<Terrain>();
        float[,,] alphamap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                float ratioX = i / (float)width;
                float ratioY = j / (float)height;
                float h = terrainData.GetSteepness(ratioY, ratioX);
                float h2 = terrainData.GetInterpolatedHeight(ratioY, ratioX);
                if(h2 < waterLevel*terrainData.heightmapResolution)
                {
                    float r = h2 / (waterLevel * terrainData.heightmapResolution);
                    if (h < 40 && h2 < texHeight) h = 0;
                    else h = 1;
                    alphamap[i, j, 0] = r*(1-h);
                    alphamap[i, j, 1] = r*(h);// 1-Mathf.PerlinNoise(i / ((float)width / 5), j / ((float)height / 5));
                    alphamap[i, j, 2] = 0;
                    alphamap[i, j, 3] = 1-r; 
                    continue;
                }
                if (h2 > texHeight)
                {
                    h2 -= texHeight;
                    h2 /= 50f;
                    h2 = Mathf.Clamp(h2, 0, 1);
                    h = Mathf.Clamp((h - (20 + h2*30f)) / 20f, 0, 1f);
                    alphamap[i, j, 0] = 0;
                    alphamap[i, j, 1] = h;
                    alphamap[i, j, 2] = (1-h);
                    alphamap[i, j, 3] = 0;
                    continue;
                }
                h = Mathf.Clamp((h - 30) / 30f, 0, 1f);
                alphamap[i, j, 0] = 1 - h;// - h;
                alphamap[i, j, 1] = h;// h;// 1-Mathf.PerlinNoise(i / ((float)width / 5), j / ((float)height / 5));
                alphamap[i, j, 2] = 0;
                alphamap[i, j, 3] = 0;
            }
        terrainData.SetAlphamaps(0, 0, alphamap);
    }

    void SetDetails(TerrainData terrainData)
    {
        Terrain terrain = GetComponent<Terrain>();
        int size = treeDensity;
        terrainData.treeInstances = new TreeInstance[1];
        float step = 1 / (float)size;
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
            {
                if (terrainData.GetSteepness(i * step, j * step) > 20 || terrainData.GetInterpolatedHeight(i * step, j * step) > texHeight || terrainData.GetInterpolatedHeight(i * step, j * step) < waterLevel*terrainData.heightmapResolution)
                    continue;
                
                TreeInstance tree = new TreeInstance();
                tree.prototypeIndex = 0;
                tree.heightScale = 1;
                tree.widthScale = 1;
                tree.position = new Vector3(i * step + Random.Range(-step / 2f, step / 2f), 0, j * step + +Random.Range(-step / 2f, step / 2f));
                terrain.AddTreeInstance(tree);
            }
        terrain.Flush();

        //Details
        int[,] details = new int[terrainData.detailWidth, terrainData.detailHeight];
        for(int i = 0; i < terrainData.detailWidth; i++)
            for(int j = 0; j < terrainData.detailHeight; j++)
            {
                float stepX = i / (float)terrainData.detailWidth;
                float stepY = j / (float)terrainData.detailHeight;
                float steepness = 90 - terrainData.GetSteepness(stepY, stepX);
                float height = terrainData.GetInterpolatedHeight(stepY, stepX);
                height = Mathf.Clamp(height, 0, texHeight)/texHeight;
                if (height < waterLevel) continue;
                if (steepness < 50) steepness = 0;
                else steepness -= 50;
                steepness /= 40f;
                details[i, j] = (int)((1-height)*(steepness*2));
            }
        terrainData.SetDetailLayer(0, 0, 0, details);
        terrain.Flush();
        //int[,] layer = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, terrainData.detailResolution);
    }

}
