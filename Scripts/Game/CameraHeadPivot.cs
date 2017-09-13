using UnityEngine;
using System.Collections;

public class CameraHeadPivot : MonoBehaviour
{
    static private CameraHeadPivot s_singleton;

    private Quaternion m_currentRotation;
    private Quaternion m_targetRotation;

    private float m_transitionTime;

	private void Awake ()
	{
	    s_singleton = this;

        m_currentRotation = m_targetRotation = transform.localRotation;
	}

    private void Update()
    {
        if (m_transitionTime <= 0)
        {
            return;
        }
       
        float ratio = m_transitionTime / GameManager.cameraTransitionTime;
        ratio *= ratio;
        ratio = 1 - ratio;
        s_singleton.transform.localRotation = Quaternion.Slerp(m_currentRotation, m_targetRotation, ratio);// = new Vector3(m_currentAngle * ratio + m_targetAngle * (1 - ratio), 0, 0);
        
        m_transitionTime -= Time.deltaTime;

        if (m_transitionTime <= 0)
        {
            m_transitionTime = 0;
            s_singleton.transform.localRotation = m_targetRotation;
        }
    }

    private void _SetCameraAngle(float angle)
    {
        m_currentRotation = transform.localRotation;
        m_targetRotation = Quaternion.Euler(angle, 0, 0);

        m_transitionTime = GameManager.cameraTransitionTime;
    }

    static public void SetCameraAngle(float angle)
    {
        s_singleton._SetCameraAngle(angle);
    }

    static public CameraHeadPivot cameraHeadPivot
    {
        get { return s_singleton; }
    }
}
