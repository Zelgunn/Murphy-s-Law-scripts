using UnityEngine;
using System.Collections;

public class QuickStart : MonoBehaviour
{
    static private QuickStart s_singleton;
    [SerializeField] private GameManager m_gameManagerPrefab;

	private void Awake ()
	{
        s_singleton = this;
        Instantiate(m_gameManagerPrefab);
	}

    static public QuickStart quickStart
    {
        get { return s_singleton; }
    }
}
