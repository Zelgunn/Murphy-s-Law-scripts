using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FairyPowerSlider : MonoBehaviour
{
    private Slider m_slider;

    private void Awake()
    {
        m_slider = GetComponent<Slider>();
    }

	private void Update ()
	{
        if (!Fairy.fairy) return;

        m_slider.value = Fairy.fairy.strength;
	}
}
