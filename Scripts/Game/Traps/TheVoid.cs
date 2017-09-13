using UnityEngine;
using System.Collections;

public class TheVoid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Protector.protector.gameObject)
        {
            if(Protector.protector.invulnerable)
            {
                Protector.protector.transform.position = StartPoint.startPoint.position;
                return;
            }

            Protector.KillProtector(false);
        }
    }
}
