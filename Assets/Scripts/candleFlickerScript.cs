using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candleFlickerScript : MonoBehaviour
{
    public float randomSeed;
    public Light light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float intensity = Mathf.PerlinNoise(Time.time + randomSeed, 0.0f);
        light.intensity = (intensity + 0.4f)* 5;
    }
}
