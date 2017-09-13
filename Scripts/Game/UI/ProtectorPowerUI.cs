using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProtectorPowerUI : MonoBehaviour
{
    [SerializeField] private RawImage m_icon;
    [SerializeField] private RawImage m_highlight;

    [SerializeField] private float m_highlightSpeed = 2;
    private float m_powerHighlightStrength = 0;
    private bool m_powerAvailableReset = false;

	private void Update ()
	{
        if (!Fairy.fairy) return;

        if(Fairy.fairy.powerAvailable)
        {
            //m_icon.color = Color.white;

            if (!m_powerAvailableReset)
            {
                m_powerHighlightStrength = 0;
                m_powerAvailableReset = true;
            }

            m_powerHighlightStrength += Time.deltaTime * m_highlightSpeed;

            float alpha = (Mathf.Sin(m_powerHighlightStrength * Mathf.PI) + 1) / 4 + 0.5f;
            m_highlight.color = new Color(1, 1, 1, alpha);
        }
        else
        {
            //m_icon.color = Color.Lerp(Color.gray, Color.white, Fairy.fairy.power);
            m_highlight.color = new Color(1, 1, 1, Fairy.fairy.power / 2);
            m_powerAvailableReset = false;
        }
	}
}
