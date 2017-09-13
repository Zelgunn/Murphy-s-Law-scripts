using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

class Cheats
{
    [MenuItem("Murphy's Law/Cheats/Unlock all Levels for current Profile")]
    public static void UnlockAllLevels()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Not in play mode.");
            return;
        }

        PlayerProfile.currentProfile.m_highestLevelAvailaible = GameManager.levelCount - 1;

        if (GameManager.inMainScreen)
        {
            MenuManager.ForceUpdateUI();
        }
    }

    [MenuItem("Murphy's Law/Cheats/Toggle Protector Invulnerability On|Off")]
    public static void GrantProtectorInvulnerability()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Not in play mode.");
            return;
        }

        if (GameManager.inMainScreen)
        {
            Debug.LogError("Not in a level.");
            return;
        }

        Protector.protector.invulnerable = !Protector.protector.invulnerable;

        if(Protector.protector.invulnerable)
        {
            Debug.Log("Protector's invulnerability : On");
        }
        else
        {
            Debug.Log("Protector's invulnerability : Off");
        }
    }


}

#endif