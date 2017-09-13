using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    static private GameManager s_singleton;
    static private bool s_isInTurotial = false;

    [Header("Levels")]
    [SerializeField] private int m_mainScreenIndex = 0;
    [SerializeField] private int m_tutorialLevelIndex = 1;
    [SerializeField] private int[] m_levelsIndexes;
    [SerializeField] private int m_creditsSceneIndex = 7;
    private int m_currentSceneIndex = - 1;

    [Header("Falcon")]
    [SerializeField] private Transform m_falconGodObject;
    [SerializeField] private float m_falconDestructionImpulseStrength = 1000;
    [SerializeField] private float m_falconDestructionImpulseDuration = 0.1f;
    [SerializeField] private Vector3 m_falconFairyMaxResistance = new Vector3(0, 1000, 1000);

    [Header("Prefabs instanciated in game")]
    [SerializeField] private Canvas m_gameUIPrefab;
    [SerializeField] private GameObject m_playersPrefab;
    private GameObject m_players;

    [Header("Parameters")]
    [SerializeField] private float m_maxCameraDistance = 3;
    [SerializeField] private float m_transitionTime = 0.25f;
    [SerializeField] private float m_timeBeforeReloadAfterDeath = 5;

    protected bool m_playerIndexSet = false;
    protected PlayerIndex m_playerIndex;
    protected GamePadState m_gamePadState;
    protected GamePadState m_prevState;

    private bool m_xButtonPressed = false;
    private bool m_xButtonDown = false;

    private bool m_aButtonPressed = false;
    private bool m_aButtonDown = false;

    private bool m_bButtonPressed = false;
    private bool m_bButtonDown = false;

    private bool m_nextButtonPressed = false;
    private bool m_nextButtonDown = false;

    private bool m_prevButtonPressed = false;
    private bool m_prevButtonDown = false;

	private void Awake ()
	{
        if(s_singleton)
        {
            Destroy(gameObject);
            return;
        }

        s_singleton = this;
        DontDestroyOnLoad(gameObject);
	}

    private void Update()
    {
        if (!m_playerIndexSet || !m_prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    m_playerIndex = testPlayerIndex;
                    m_playerIndexSet = true;
                }
            }
        }

        m_prevState = m_gamePadState;
        m_gamePadState = GamePad.GetState(m_playerIndex);

        m_xButtonDown = !m_xButtonPressed && (m_gamePadState.Buttons.X == ButtonState.Pressed);
        m_xButtonPressed = m_gamePadState.Buttons.X == ButtonState.Pressed;

        m_aButtonDown = !m_aButtonPressed && (m_gamePadState.Buttons.A == ButtonState.Pressed);
        m_aButtonPressed = m_gamePadState.Buttons.A == ButtonState.Pressed;

        m_bButtonDown = !m_bButtonPressed && (m_gamePadState.Buttons.B == ButtonState.Pressed);
        m_bButtonPressed = m_gamePadState.Buttons.B == ButtonState.Pressed;

        bool nextPressed = (m_gamePadState.DPad.Right == ButtonState.Pressed) || (m_gamePadState.DPad.Up == ButtonState.Pressed);
        m_nextButtonDown = !m_nextButtonPressed && nextPressed;
        m_nextButtonPressed = nextPressed;

        bool prevPressed = (m_gamePadState.DPad.Left == ButtonState.Pressed) || (m_gamePadState.DPad.Down == ButtonState.Pressed);
        m_prevButtonDown = !m_prevButtonPressed && prevPressed;
        m_prevButtonPressed = prevPressed;

        if (Protector.protector && !(GameUIManager.manager.pauseMenuShown || GameUIManager.manager.scoreMenuShown))
        {
            Protector.protector.UpdateControls(m_gamePadState);
        }

        if((m_currentSceneIndex == m_creditsSceneIndex) && (m_xButtonDown || m_aButtonDown || m_bButtonDown))
        {
            ReturnToMainScreen();
        }
    }

    #region Main Screen
    static public void ReturnToMainScreen()
    {
        if(!s_singleton)
        {
            Debug.LogError("GameManager::ReturnToMainScreen() : Game Manager singleton not initialized");
            return;
        }

        PlayerProfile.SaveProfiles();
        SceneManager.LoadScene(s_singleton.m_mainScreenIndex);
    }

    private bool _inMainScreen
    {
        get { return m_currentSceneIndex == m_mainScreenIndex; }
    }

    public static bool inMainScreen
    {
        get
        {
            return s_singleton && s_singleton._inMainScreen;
        }
    }
    #endregion

    #region Levels
    static public void CompleteLevel()
    {
        if (!PlayerProfile.loaded)
        {
            PlayerProfile.LoadProfiles();
        }

        int currentMaxLevel = PlayerProfile.currentProfile.m_highestLevelAvailaible;
        if (currentMaxLevel == OrderOfCurrentLevel())
        {
            PlayerProfile.currentProfile.UnlockNextLevel();
        }

        AudioManager.PlayMusic(AudioManager.victoryMusic);
        GameUIManager.manager.ShowScoreMenu();
    }

    static public void LoadLevel(int level)
    {
        if ((level < 0) || (level >= levelCount))
        {
            Debug.LogError("GameManager::LoadLevel : level (" + level + ") is out of bounds ([0," + levelCount + "]).");
            return;
        }

        SceneManager.LoadScene(s_singleton.m_levelsIndexes[level]);
    }

    static public void LoadNextLevel()
    {
        int level = OrderOfCurrentLevel() + 1;
        LoadLevel(level);
    }

    private int _levelCount
    {
        get
        {
            if (s_singleton.m_levelsIndexes == null) return 0;
            return m_levelsIndexes.Length;
        }
    }

    static public int levelCount
    {
        get 
        {
            if (!s_singleton) return 0;
            return s_singleton._levelCount;
        }
    }

    static public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator StartLoseSequenceCoroutine(bool useLoseSequenceLenght)
    {
        AudioManager.PlayOneShot(AudioManager.protectorDeathSound);

        if (useLoseSequenceLenght) yield return new WaitForSeconds(m_timeBeforeReloadAfterDeath);
        else yield return new WaitForSeconds(0.5f);

        if(s_isInTurotial)
        {
            Protector.protector.transform.position = TutorialCheckpoint.LastRegisteredCheckpoint().position;
            Protector.protector.transform.rotation = TutorialCheckpoint.LastRegisteredCheckpoint().rotation;
            Protector.ReviveProtector();
        }
        else
        {
            ReloadLevel();
        }
    }

    static public void StartLoseSequence(bool useLoseSequenceLenght)
    {
        s_singleton.StartCoroutine(s_singleton.StartLoseSequenceCoroutine(useLoseSequenceLenght));
    }

    static public int OrderOfCurrentLevel()
    {
        return s_singleton.OrderOfLevel(SceneManager.GetActiveScene());
    }

    private int OrderOfLevel(Scene level)
    {
        for(int i = 0; i < m_levelsIndexes.Length; i++)
        {
            if (m_levelsIndexes[i] == level.buildIndex)
            {
                return i;
            }
        }

        return -1;
    }

    static public bool loadedLevelIsFinal
    {
        get
        {
            if (!s_singleton) return false;

            return OrderOfCurrentLevel() == (levelCount - 1);
        }
    }
    #endregion

    static public void LoadTutorial()
    {
        SceneManager.LoadScene(s_singleton.m_tutorialLevelIndex);
    }

    static public void LoadCredits()
    {
        SceneManager.LoadScene(s_singleton.m_creditsSceneIndex);
    }

    #region Signals
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        s_isInTurotial = false;
        m_currentSceneIndex = scene.buildIndex;

        if (scene.buildIndex == m_mainScreenIndex)
        {
            OnMainScreenLoaded();
        }
        else if (scene.buildIndex == m_tutorialLevelIndex)
        {
            OnTutorialLoaded();
        }
        else if (scene.buildIndex == m_creditsSceneIndex)
        {
            OnCreditsLoaded();
        }
        else
        {
            OnGameLevelLoaded();
        }
    }

    private void OnMainScreenLoaded()
    {
        PlayerProfile.LoadProfiles();
        AudioManager.PlayMusic(AudioManager.mainScreenMusic);
    }

    private void OnGameLevelLoaded()
    {
        Instantiate(m_gameUIPrefab);

        m_players = Instantiate(m_playersPrefab);
        if (QuickStart.quickStart)
        {
            m_players.transform.position = QuickStart.quickStart.transform.position;
            m_players.transform.rotation = QuickStart.quickStart.transform.rotation;

        }
        else if(StartPoint.startPoint)
        {
            m_players.transform.position = StartPoint.startPoint.position;
            m_players.transform.rotation = StartPoint.startPoint.rotation;
        }
        else
        {
            Debug.LogError("Aucun point de départ n'a été défini. Les joueurs apparaîtront en (0,0,0).");
        }

        CameraHeadOffset cameraHeadOffset = Camera.main.gameObject.AddComponent<CameraHeadOffset>();
        cameraHeadOffset.maxCameraDistance = m_maxCameraDistance;

        AudioManager.PlayMusic(AudioManager.gameAmbiance);

        Time.timeScale = 1;
    }

    private void OnTutorialLoaded()
    {
        OnGameLevelLoaded();

        s_isInTurotial = true;
    }

    private void OnCreditsLoaded()
    {
        AudioManager.PlayMusic(AudioManager.creditsMusic);
    }

    private void OnApplicationQuit()
    {
        PlayerProfile.SaveProfiles();
    }
    #endregion

    #region World Data
    static public float cameraTransitionTime
    {
        get { return s_singleton.m_transitionTime; }
    }

    static public Transform falconGodObject
    {
        get { return s_singleton.m_falconGodObject; }
    }

    static public float falconDestructionImpulseStrength
    {
        get { return s_singleton.m_falconDestructionImpulseStrength; }
    }

    static public float falconDestructionImpulseDuration
    {
        get { return s_singleton.m_falconDestructionImpulseDuration; }
    }

    static public Vector3 falconFairyMaxResistance
    {
        get { return s_singleton.m_falconFairyMaxResistance; }
    }

    static public GamePadState gamePadState
    {
        get { return s_singleton.m_gamePadState; }
    }

    static public bool GetXDown()
    {
        return s_singleton.m_xButtonDown || Input.GetKeyDown(KeyCode.Escape);
    }

    static public bool GetADown()
    {
        return s_singleton.m_aButtonDown;
    }

    static public bool GetBDown()
    {
        return s_singleton.m_bButtonDown;
    }

    static public bool GetNextDown()
    {
        return s_singleton.m_nextButtonDown;
    }

    static public bool GetPrevDown()
    {
        return s_singleton.m_prevButtonDown;
    }

    #endregion
}
