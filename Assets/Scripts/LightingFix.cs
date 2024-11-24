using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingFix : MonoBehaviour
{
    void Start()
    {
        DynamicGI.UpdateEnvironment();

        foreach (Light light in FindObjectsOfType<Light>())
        {
            light.enabled = false;
            light.enabled = true;
        }
    }
}

