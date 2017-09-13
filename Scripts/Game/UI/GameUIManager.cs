using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    static private GameUIManager s_singleton;

    [SerializeField] private GameObject m_mainUI;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_scoreMenu;

    [Header("Main UI")]
    [SerializeField] private GraphicTimer m_timer;

    [Header("Score Menu")]
    [SerializeField] private Text m_timeScoreField;
    [SerializeField] private Text m_gatheredBonusesScoreField;
    [SerializeField] private Text m_destroyedElementsScoreField;
    [SerializeField] private Text m_finalScoreScoreField;
    [Space(5)]
    [SerializeField] private Button m_nextLevelButton;
    [SerializeField] private Button m_creditsButton;

    private void Awake()
    {
        if(s_singleton)
        {
            Debug.LogWarning("Erreur : Deux GameUIManager présents dans la scène.");
            Destroy(gameObject);
            return;
        }

        s_singleton = this;
    }

    private void Update()
    {
        if(GameManager.GetXDown())
        {
            if (scoreMenuShown)
            {
                Time.timeScale = 1;
                GameManager.ReturnToMainScreen();
            }
            else
            {
                if (pauseMenuShown)
                {
                    GameManager.ReturnToMainScreen();
                }
                else
                {
                    ShowPauseMenu();
                }

            }
        }

        if (GameManager.GetADown())
        {
            if(scoreMenuShown)
            {
                GameManager.LoadNextLevel();
            }
            else if(pauseMenuShown)
            {
                OnResume();
            }
        }

        if(GameManager.GetBDown())
        {
            if(scoreMenuShown || pauseMenuShown)
            {
                Application.Quit();
            }
        }
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0;
        m_pauseMenu.gameObject.SetActive(true);
        m_scoreMenu.gameObject.SetActive(false);
    }

    public bool pauseMenuShown
    {
        get { return m_pauseMenu.gameObject.activeSelf; }
    }

    public void OnResume()
    {
        Time.timeScale = 1;
        m_pauseMenu.gameObject.SetActive(false);
    }

    public void OnReturnToMainScreen()
    {
        Time.timeScale = 1;
        GameManager.ReturnToMainScreen();
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnCredits()
    {
        Time.timeScale = 1;
        GameManager.LoadCredits();
    }

    public void OnGoToNextLevel()
    {
        Time.timeScale = 1;
        GameManager.LoadNextLevel();
    }

    public void ShowScoreMenu()
    {
        Time.timeScale = 0;

        m_pauseMenu.gameObject.SetActive(false);
        m_scoreMenu.gameObject.SetActive(true);

        m_timeScoreField.text = m_timer.formatedTime;
        m_gatheredBonusesScoreField.text = Protector.protector.gatheredBonuses.ToString();
        m_destroyedElementsScoreField.text = Fairy.fairy.destroyedElements.ToString();

        float timeScoreContribution = 60 / (m_timer.time + 60);
        int score = (int) (100 * (1 + Protector.protector.gatheredBonuses) * (1 + Fairy.fairy.destroyedElements) * timeScoreContribution);

        m_finalScoreScoreField.text = score.ToString();

        if(GameManager.loadedLevelIsFinal)
        {
            m_creditsButton.gameObject.SetActive(true);
            m_nextLevelButton.gameObject.SetActive(false);
        }
    }

    public bool scoreMenuShown
    {
        get { return m_scoreMenu.gameObject.activeSelf; }
    }

    static public GameUIManager manager
    {
        get { return s_singleton; }
    }
}
