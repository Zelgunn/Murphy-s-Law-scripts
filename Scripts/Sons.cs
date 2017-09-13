using UnityEngine;
using System.Collections;

public class Sons : MonoBehaviour
{
    bool m_played = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject == Protector.protector.gameObject) && !m_played)
        {
            GetComponent<AudioSource>().Play();
            m_played = true;
        }
    }
    
}
