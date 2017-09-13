using UnityEngine;
using System.Collections;

public class CameraHeadOffset : MonoBehaviour
{
    static private CameraHeadOffset s_singleton;

    private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter m_character;
    private Transform m_pivot;
    private float m_maxCameraDistance;
    private float m_maxCameraDistanceSource;
    private float m_maxCameraDistanceTarget;
    private float m_transitionTime;

    private int m_cameraRaycastLayers;

    private void Awake()
    {
        m_cameraRaycastLayers = 1 << LayerMask.NameToLayer("Fairy");
        m_cameraRaycastLayers |= 1 << LayerMask.NameToLayer("Ignore Raycast");
        m_cameraRaycastLayers |= 1 << LayerMask.NameToLayer("Destructible");
        m_cameraRaycastLayers = ~m_cameraRaycastLayers;

        s_singleton = this;
    }

    private void Start()
    {
        m_character = Protector.protector.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();
        m_pivot = CameraHeadPivot.cameraHeadPivot.transform;
    }

    private void Update ()
    {
        UpdateMaxDistance();
        UpdatePhysicalCamera();
	}

    private void UpdateMaxDistance()
    {
        if (m_transitionTime <= 0)
        {
            return;
        }

        float ratio = m_transitionTime / GameManager.cameraTransitionTime;
        ratio *= ratio;
        m_maxCameraDistance = m_maxCameraDistanceSource * ratio + m_maxCameraDistanceTarget * (1 - ratio);

        m_transitionTime -= Time.deltaTime;
        if(m_transitionTime <= 0)
        {
            m_transitionTime = 0;
            m_maxCameraDistance = m_maxCameraDistanceTarget;
        }
    }

    private void UpdatePhysicalCamera()
    {
        RaycastHit raycastHit;
        Ray rayFromPivot = new Ray(m_pivot.position, -m_pivot.forward);

        float cameraDistance = Mathf.Lerp(0.9f, 1, m_character.speed) * m_maxCameraDistance;

        if (Physics.Raycast(rayFromPivot, out raycastHit, Mathf.Infinity, m_cameraRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            float distanceFromPivotToPoint = (raycastHit.point - m_pivot.position).magnitude;

            if (distanceFromPivotToPoint >= cameraDistance)
            {
                transform.position = m_pivot.position - m_pivot.forward * cameraDistance;
            }
            else
            {
                transform.position = raycastHit.point;
            }
        }
        else
        {
            transform.position = m_pivot.position - m_pivot.forward * cameraDistance;
        }

        transform.rotation = m_pivot.rotation;
    }

    public float maxCameraDistance
    {
        get { return m_maxCameraDistance; }
        set { m_maxCameraDistance = m_maxCameraDistanceSource = m_maxCameraDistanceTarget = value; }
    }

    private void _SetMaxCameraDistance(float cameraDistance)
    {
        m_maxCameraDistanceSource = m_maxCameraDistance;
        m_maxCameraDistanceTarget = cameraDistance;

        m_transitionTime = GameManager.cameraTransitionTime;
    }

    static public void SetMaxCameraDistance(float cameraDistance)
    {
        s_singleton._SetMaxCameraDistance(cameraDistance);
    }

    public Transform pivot
    {
        get { return m_pivot; }
        set { m_pivot = value; }
    }
}
