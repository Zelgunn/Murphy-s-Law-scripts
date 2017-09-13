using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public enum MainScreenMenu
    {
        StartMenu,
        LevelSelectionMenu,
        SavesMenu,
        None
    }

    static private MenuManager s_singleton;

    [Header("Menus")]
    [SerializeField] private GameObject m_startMenu;
    [SerializeField] private GameObject m_levelsMenu;
    [SerializeField] private GameObject m_savesMenu;

    [Header("Level selection menu")]
    [SerializeField] private Button[] m_levelButtons;
    private Image[] m_levelVignettes;
    [SerializeField] private Button m_tutorialButton;

    [Header("Save selection menu")]
    [SerializeField] private Image[] m_saveIcons;
    [SerializeField] private Text m_currentSaveText;

    private MainScreenMenu m_currentMenu = MainScreenMenu.StartMenu;
    private int m_selectedLevel = -1;
    private int m_selectedSave = 0;

    private void Awake()
    {
        s_singleton = this;

        m_levelVignettes = new Image[m_levelButtons.Length];
        for(int i = 0; i <m_levelButtons.Length; i++)
        {
            m_levelVignettes[i] = m_levelButtons[i].gameObject.GetComponent<Image>();
        }
    }

    private void Update()
    {
        if(GameManager.GetXDown())
        {
            OnReturn();
        }

        if(GameManager.GetADown())
        {
            switch(m_currentMenu)
            {
                case MainScreenMenu.StartMenu:
                    OnPlay();
                    break;
                case MainScreenMenu.LevelSelectionMenu:
                    if (m_selectedLevel == -1)
                    {
                        OnTutorial();
                    }
                    else
                    {
                        OnPlayLevel(m_selectedLevel);
                    }
                    break;
                case MainScreenMenu.SavesMenu:
                    OnSaveSelected(m_selectedSave);
                    break;
            }
        }

        if(GameManager.GetBDown())
        {
            switch (m_currentMenu)
            {
                case MainScreenMenu.StartMenu:
                    OnCredits();
                    break;
                case MainScreenMenu.LevelSelectionMenu:
                    OnProfiles();
                    break;
                case MainScreenMenu.SavesMenu:
                    OnReturn();
                    break;
            }
        }

        if(GameManager.GetNextDown())
        {
            switch (m_currentMenu)
            {
                case MainScreenMenu.LevelSelectionMenu:
                    SelectNextLevel();
                    break;
                case MainScreenMenu.SavesMenu:
                    SelectNextSave();
                    break;
            }
        }

        if(GameManager.GetPrevDown())
        {
            switch (m_currentMenu)
            {
                case MainScreenMenu.LevelSelectionMenu:
                    SelectPrevLevel();
                    break;
                case MainScreenMenu.SavesMenu:
                    SelectPrevSave();
                    break;
            }
        }
    }

    private void SelectNextLevel()
    {
        m_selectedLevel++;

        PlayerProfile playerProfile = PlayerProfile.currentProfile;
        if (m_selectedLevel > playerProfile.m_highestLevelAvailaible) m_selectedLevel = -1;

        UpdateLevelSelectionMenu();
    }

    private void SelectPrevLevel()
    {
        m_selectedLevel--;

        PlayerProfile playerProfile = PlayerProfile.currentProfile;
        if (m_selectedLevel <= -2)
        {
            m_selectedLevel = playerProfile.m_highestLevelAvailaible;
        }

        UpdateLevelSelectionMenu();
    }

    private void SelectNextSave()
    {
        m_selectedSave++;
        if (m_selectedSave >= PlayerProfile.profilesCount) m_selectedSave = 0;

        UpdateSaveSelectionMenu();
    }

    private void SelectPrevSave()
    {
        m_selectedSave--;
        if (m_selectedSave < 0) m_selectedSave = PlayerProfile.profilesCount - 1;

        UpdateSaveSelectionMenu();
    }

    #region Buttons Slots
    public void OnPlay()
    {
        ShowMenu(MainScreenMenu.LevelSelectionMenu);
    }

    public void OnProfiles()
    {
        ShowMenu(MainScreenMenu.SavesMenu);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnPlayLevel(int level)
    {
        AudioManager.PlayClicSound();
        GameManager.LoadLevel(level);
    }

    public void OnReturn()
    {
        switch (m_currentMenu)
        {
            case MainScreenMenu.StartMenu:
                break;
            case MainScreenMenu.LevelSelectionMenu:
                ShowMenu(MainScreenMenu.StartMenu);
                break;
            case MainScreenMenu.SavesMenu:
                ShowMenu(MainScreenMenu.LevelSelectionMenu);
                break;
        }
    }

    public void OnSaveSelected(int selectedSave)
    {
        PlayerProfile.currentProfileIndex = selectedSave;
        ShowMenu(MainScreenMenu.LevelSelectionMenu);
    }

    public void OnTutorial()
    {
        AudioManager.PlayClicSound();
        GameManager.LoadTutorial();
    }

    public void OnCredits()
    {
        AudioManager.PlayClicSound();
        GameManager.LoadCredits();
    }
    #endregion

    private void UpdateLevelSelectionMenu()
    {
        PlayerProfile playerProfile = PlayerProfile.currentProfile;

        for (int i = 0; i < m_levelButtons.Length; i++)
        {
            m_levelButtons[i].interactable = playerProfile.m_highestLevelAvailaible >= i;

            if (m_levelButtons[i].interactable && (i != m_selectedLevel))
            {
                m_levelButtons[i].image.color = new Color(1, 1, 1, 0.75f);
            }
            else
            {
                m_levelButtons[i].image.color = Color.white;
            }
        }

        if (m_selectedLevel == -1)
        {
            m_tutorialButton.image.color = Color.white;
        }
        else
        {
            m_tutorialButton.image.color = new Color(1, 1, 1, 0.75f);
        }

        m_currentSaveText.text = "Sauvegarde n°" + (PlayerProfile.currentProfileIndex + 1);
    }

    private void UpdateSaveSelectionMenu()
    {
        for(int i = 0; i < m_saveIcons.Length; i++)
        {
            int level = PlayerProfile.GetProfile(i).m_highestLevelAvailaible;
            m_saveIcons[i].sprite = m_levelVignettes[level].sprite;

            if(i == m_selectedSave)
            {
                m_saveIcons[i].color = Color.white;
            }
            else
            {
                m_saveIcons[i].color = new Color(1, 1, 1, 0.75f);
            }
        }
    }

    public void ShowMenu(MainScreenMenu menu)
    {
        m_currentMenu = menu;

        switch (menu)
        {
            case MainScreenMenu.StartMenu:
                break;
            case MainScreenMenu.LevelSelectionMenu:
                UpdateLevelSelectionMenu();
                break;
            case MainScreenMenu.SavesMenu:
                UpdateSaveSelectionMenu();
                break;
        }

        m_startMenu.SetActive(menu == MainScreenMenu.StartMenu);
        m_levelsMenu.SetActive(menu == MainScreenMenu.LevelSelectionMenu);
        m_savesMenu.SetActive(menu == MainScreenMenu.SavesMenu);

        AudioManager.PlayClicSound();
    }

    static public void ForceUpdateUI()
    {
        if (!s_singleton) return;

        s_singleton.UpdateLevelSelectionMenu();
        s_singleton.UpdateSaveSelectionMenu();
    }
}
