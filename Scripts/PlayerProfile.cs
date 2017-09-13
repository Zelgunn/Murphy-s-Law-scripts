using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

public class PlayerProfile
{
    private const int s_profilesCount = 8;

    static private string s_savePath = "Save.xml";
    static private PlayerProfile[] s_profiles;
    static private int s_currentProfileIndex = 0;
    static private bool s_loaded = false;

    public int m_profileID;
    public int m_highestLevelAvailaible = 0;

    public PlayerProfile()
    {

    }

    public PlayerProfile(int id)
    {
        m_profileID = id;
    }

    public void UnlockNextLevel()
    {
        m_highestLevelAvailaible++;
        if (m_highestLevelAvailaible >= GameManager.levelCount) m_highestLevelAvailaible = GameManager.levelCount - 1;
    }

    static public int profilesCount
    {
        get { return s_profilesCount; }
    }

    static public int currentProfileIndex
    {
        get { return s_currentProfileIndex; }
        set
        {
            if ((value >= 0) && (value < s_profilesCount))
            {
                s_currentProfileIndex = value;
            }
        }
    }

    static public PlayerProfile currentProfile
    {
        get { return s_profiles[s_currentProfileIndex]; }
    }

    static public PlayerProfile GetProfile(int profileID)
    {
        if ((profileID >= s_profilesCount) || (profileID < 0))
            return null;

        return s_profiles[profileID];
    }

    static public void LoadProfiles()
    {
        if (!System.IO.File.Exists(s_savePath))
        {
            // Creation des profiles
            s_profiles = new PlayerProfile[s_profilesCount];
            for (int i = 0; i < s_profilesCount; i++)
            {
                s_profiles[i] = new PlayerProfile(i);
            }
        }
        else
        {

            // Chargement des profiles
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerProfile[]));

            using (StreamReader streamReader = new StreamReader(s_savePath))
            {
                s_profiles = xmlSerializer.Deserialize(streamReader) as PlayerProfile[];
            }

            if ((s_profiles == null) || (s_profiles.Length < s_profilesCount))
            {
                int currentLenght = 0;
                if(s_profiles != null)
                {
                    currentLenght = s_profiles.Length;
                }
                PlayerProfile[] tmpProfiles = new PlayerProfile[s_profilesCount];
                for(int i = 0; i < s_profilesCount; i++)
                {
                    if (i < currentLenght)
                    {
                        tmpProfiles[i] = s_profiles[i];
                    }
                    else
                    {
                        tmpProfiles[i] = new PlayerProfile(i);
                    }
                }
                s_profiles = tmpProfiles;
            }
        }

        s_loaded = true;
    }

    static public void SaveProfiles()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerProfile[]));

        using (StreamWriter streamWriter = new StreamWriter(s_savePath))
        {
            xmlSerializer.Serialize(streamWriter, s_profiles);
        }
    }

    static public bool loaded
    {
        get { return s_loaded; }
    }
}
