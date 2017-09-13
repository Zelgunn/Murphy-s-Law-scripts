using UnityEngine;
using System.Collections;

public interface ITrap
{
    void Trigger();
    void Rearm(float timeToRearm);
}