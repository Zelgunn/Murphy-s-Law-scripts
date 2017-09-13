using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class DynamicCameraArea : MonoBehaviour
{
    [SerializeField] private float m_maxDistance;
    [SerializeField] private float m_cameraAngle;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != Protector.protector.gameObject) return;

        CameraHeadOffset.SetMaxCameraDistance(m_maxDistance);
        CameraHeadPivot.SetCameraAngle(m_cameraAngle);
    }
}
