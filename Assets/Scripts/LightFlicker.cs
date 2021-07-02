using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float flikerMim = 0.25f;
    public float flikerMax = 0.3f;
    [Space]

    [Space]
    public float flikerMimTime = 0.05f;
    public float flikerMaxTime = 0.1f;
    [Space]

    [Space]
    public float rangeMim = 9.5f;
    public float rangeMax = 10f;

    private Light torchLight;

    private float currentFlickerTime = 0f;
    private float flickerTimer = 0f;

    void Start() {
        torchLight = GetComponent<Light>();
    }


    void Update()
    {
        if (currentFlickerTime >= flickerTimer) {
            currentFlickerTime += Time.deltaTime;
        }
        else {
            torchLight.intensity = Random.Range(flikerMim, flikerMax);
            torchLight.range = Random.Range(rangeMim, rangeMax);

            currentFlickerTime = 0f;
            flickerTimer = Random.Range(flikerMimTime, flikerMaxTime);
        }

    }
}
