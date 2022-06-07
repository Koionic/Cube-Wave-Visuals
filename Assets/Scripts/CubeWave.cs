using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWave : MonoBehaviour
{
    private float angle;

    public Vector3 centerPosition;
    public Vector3 centerScale;
    
    [Range(0f, 0.02f)]
    public float angleSpeed = 0.01f;

    public GameObject cubePrefab;
    
    [Header("Remember that the amount spawned will be one more than whats inputted here!")]
    public Vector2Int cubeAmount;
    public Vector2 cubeSize;

    public List<Oscillator> cubes = new List<Oscillator>();
    public List<MeshFilter> cubeFilters = new List<MeshFilter>();

    public OrbController orbController;

    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < cubeAmount.x; y++)
        {
            for (int x = 0; x < cubeAmount.y; x++)
            {
                GameObject newCube = Instantiate(cubePrefab);

                //newCube.transform.localScale = new Vector3(cubeSize.x, cubeSize.y, 1);

                float cubePosX = (cubeSize.x * x) - (((cubeAmount.x - 1) * cubeSize.x) / 2);
                float cubePosY = (cubeSize.y * y) - (((cubeAmount.y - 1) * cubeSize.y) / 2);
                
                newCube.transform.position = new Vector3(cubePosX, cubePosY, 0);

                newCube.transform.parent = transform;
                
                var d = (new Vector3(x,y,0) - centerPosition).magnitude % (((cubeAmount.x - 1) * cubeSize.x) / 2);
                var offset = MathHelper.Map(d, 0, (((cubeAmount.x - 1) * cubeSize.x) / 2), -Mathf.PI, Mathf.PI);
                var a = angle + offset;
                var h = MathHelper.Map(Mathf.Sin(a), -1, 1, 1, 10);
                
                Oscillator newOsc =  newCube.GetComponent<Oscillator>();

                if (!newOsc)
                {
                    newOsc =  newCube.AddComponent<Oscillator>();
                }
                
                newOsc.Init(d, offset, a, h, (cubeAmount.x - 1) * cubeSize.x, this);
                
                cubes.Add(newOsc);
                cubeFilters.Add(newCube.GetComponent<MeshFilter>());
            }
        }
        
        if (orbController && orbController.isActiveAndEnabled)
        {
            orbController.FeedMeshFilters(cubeFilters);

            for (int i = 0; i < cubeFilters.Count; i++)
            {
                cubeFilters[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].UpdateAngle(angle);

            angle += angleSpeed * Time.deltaTime;
        }

        // for (int y = 0; y < cubeAmount.x; y++)
        // {
        //     for (int x = 0; x < cubeAmount.y; x++)
        //     {
        //         var d = new Vector3(x,y,0).magnitude;
        //         var offset = MathHelper.Map(d, 0, (((cubeAmount.x - 1) * cubeSize.x) / 2), -Mathf.PI, Mathf.PI);
        //         var a = angle + offset;
        //         var h = Mathf.Floor(MathHelper.Map(Mathf.Sin(a), -1, 1, 10, 30));
        //         
        //         cubes[(y * cubeAmount.x) +  x].transform.localScale = new Vector3(cubeSize.x, cubeSize.y, h);
        //         
        //         print((y * cubeAmount.x) +  x);
        //     }
        // }
    }
}
