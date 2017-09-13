using UnityEngine;
using System.Collections;

public class GravitronBonus : MonoBehaviour {

    public Vector3 vitesse;
    public GameObject centreGravité;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.parent.Rotate(vitesse, Space.Self);

	}
}
