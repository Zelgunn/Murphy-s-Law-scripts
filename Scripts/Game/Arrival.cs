using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Arrival : MonoBehaviour
{
    static private Arrival s_singleton;
    private Collider m_collider;

	private void Awake ()
	{
        if(s_singleton)
        {
            Debug.LogWarning("Il y a plusieurs arrivées. Destruction des arrivées en trop.");
            Destroy(gameObject);
            return;
        }

        s_singleton = this;

        m_collider = GetComponent<Collider>();
        if(!m_collider)
        {
            Debug.LogError("Pas de collider sur l'arrivée. Destruction de l'arrivée.");
            s_singleton = null;
            Destroy(gameObject);
            return;
        }

        m_collider.isTrigger = true;
	}

	private void OnTriggerEnter (Collider collider)
	{
	    if(collider.gameObject == Protector.protector.gameObject)
        {
            GameManager.CompleteLevel();
        }
	}
}
