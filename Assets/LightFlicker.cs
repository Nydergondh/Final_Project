using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float flikerMim = 0.25f;
    public float flikerMax = 0.3f;

    public float rangeMim = 9.5f;
    public float rangeMax = 10f;

    public float maxVariation = 0.1f;
    private Light torchLight;

    void Start() {
        torchLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        torchLight.intensity = Random.Range(flikerMim, flikerMax);
        torchLight.range = Random.Range(rangeMim, rangeMax);
    }
}
