using UnityEngine;
using System.Collections;

public class LightToCam : MonoBehaviour
{
    private Light m_light;
    private float m_baseLightIntensity;

	private void Awake ()
	{
        m_light = GetComponent<Light>();
        m_baseLightIntensity = m_light.intensity;
	}

	private void Update ()
	{
        float distanceToCamera = (transform.position - Camera.main.transform.position).magnitude;

        float factor = 1;
        if (distanceToCamera < 10)
        {
            factor = 1;
        }
        else if(distanceToCamera > 50)
        {
            factor = 0;
        }
        else
        {
            factor = (50 - distanceToCamera) / 40;
        }

        m_light.intensity = m_baseLightIntensity * factor;
	}
}
