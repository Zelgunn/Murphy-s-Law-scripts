using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProtectorLivesUI : MonoBehaviour
{
    private bool m_initialized = false;
    private RawImage[] m_lifeIcons;

    private RawImage m_rawImage;

	private void Awake ()
	{
        m_rawImage = GetComponentInChildren<RawImage>();
        m_initialized = TryInitialize();
	}

	private void Update ()
	{
	    if(!m_initialized)
        {
            m_initialized = TryInitialize();
        }

        if (!m_initialized) return;

        for (int i = 0; i < m_lifeIcons.Length; i++)
        {

            if (i < Protector.protector.lives)
            {
                m_lifeIcons[i].color = Color.white;
            }
            else
            {
                m_lifeIcons[i].color = Color.gray;
            }
        }

        for (int i = 1; i < m_lifeIcons.Length; i++)
        {
            m_lifeIcons[i].transform.position = m_rawImage.transform.position + Vector3.right * i * m_rawImage.rectTransform.rect.width * 1.1f;
        }
	}

    private bool TryInitialize()
    {
        if (!Protector.protector) return false;
        if (Protector.protector.maxLives <= 1)
        {
            Destroy(gameObject);
            return true;
        }

        m_lifeIcons = new RawImage[Protector.protector.maxLives];
        m_lifeIcons[0] = m_rawImage;
        for (int i = 1; i < m_lifeIcons.Length; i++)
        {
            RawImage lifeIcon = Instantiate<RawImage>(m_rawImage);

            lifeIcon.name = "Vie n°" + (i + 1);
            lifeIcon.transform.SetParent(transform);
            lifeIcon.rectTransform.sizeDelta = m_rawImage.rectTransform.sizeDelta;

            m_lifeIcons[i] = lifeIcon;
        }

        return true;
    }
}
