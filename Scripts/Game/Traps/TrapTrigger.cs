using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private bool m_automaticReactivation = false;
    [SerializeField] private float m_timeBeforeReactivation = 5;
    [SerializeField] private GameObject m_trapToTrigger;

    private ITrap m_trapInterface;
    private bool m_activated = true;

    private void Start()
    {
        if(m_trapToTrigger == null)
        {
            enabled = false;
        }
        else
        {
            m_trapInterface = m_trapToTrigger.GetComponent<ITrap>();
            if (m_trapInterface == null)
            {
                enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != Protector.protector.gameObject) return;

        if (!m_activated) return;

        m_activated = false;
        m_trapInterface.Trigger();

        if(m_automaticReactivation)
        {
            if(m_timeBeforeReactivation > 0)
            {
                StartCoroutine(ReactivateCoroutine());
            }
            else
            {
                m_activated = true;
            }
        }
    }

    private IEnumerator ReactivateCoroutine()
    {
        yield return m_timeBeforeReactivation;
        m_activated = true;
    }
}
