using UnityEngine;
using System.Collections;

public class StartPoint : MonoBehaviour
{
    static private StartPoint s_singleton;

	private void Awake ()
	{
        s_singleton = this;
	}

    static public Transform startPoint
    {
        get
        {
            if(s_singleton)
            {
                return s_singleton.transform;
            }
            else
            {
                return null;
            }
        }
    }
}
