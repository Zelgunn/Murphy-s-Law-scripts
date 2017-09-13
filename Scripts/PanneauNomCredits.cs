using UnityEngine;
using System.Collections;

public class PanneauNomCredits : MonoBehaviour
{
	private void Update ()
	{
        transform.LookAt(transform.position * 2 - Camera.main.transform.position);
	}
}
