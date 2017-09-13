using UnityEngine;
using System.Collections;

public class Token : MonoBehaviour
{
    private Vector3 m_basePosition;

    private void Awake()
    {
        m_basePosition = transform.position;
    }

    private void Update()
    {
        transform.position = m_basePosition + Vector3.up * Mathf.Sin(Time.time) * 0.2f;
        transform.Rotate(Vector3.up, Time.deltaTime * 36, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Protector.protector.gameObject) return;

        Protector.protector.gatheredBonuses++;
        Fairy.fairy.Regenerate();

        Destroy(gameObject);
    }
}
