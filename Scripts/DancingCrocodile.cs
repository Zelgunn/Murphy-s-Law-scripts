using UnityEngine;
using System.Collections;

public class DancingCrocodile : MonoBehaviour
{
    [SerializeField] private bool m_startDancingAtAwake = false;
    [SerializeField] private bool m_dancing = false;
    private Animator m_animator;

	private void Awake ()
	{
        m_animator = GetComponent<Animator>();

        if (m_startDancingAtAwake) StartDancing();
	}

    private void Update()
    {
        if (m_dancing) StartDancing();
    }

    public void StartDancing()
    {
        m_animator.SetTrigger("TriggerAnim");
    }
}
