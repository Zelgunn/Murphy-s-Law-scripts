using UnityEngine;
using System.Collections;

public class TutorialCheckpoint : MonoBehaviour
{
    static private TutorialCheckpoint s_currentCheckPoint;
    private bool m_registered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (m_registered) return;
        if(other.gameObject == Protector.protector.gameObject)
        {
            RegisterAsCurrentCheckPoint();
        }
    }

    public void RegisterAsCurrentCheckPoint()
    {
        s_currentCheckPoint = this;
        m_registered = true;
    }

    static public Transform LastRegisteredCheckpoint()
    {
        if (!s_currentCheckPoint) return StartPoint.startPoint;
        return s_currentCheckPoint.transform;
    }
}
