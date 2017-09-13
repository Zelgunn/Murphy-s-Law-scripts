using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class Protector : MonoBehaviour
{
    static private Protector s_singleton;

    private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter m_character;
    private Transform m_Cam;
    private Vector3 m_CamForward; 
    private Vector3 m_Move;
    private bool m_Jump = false;

    [SerializeField] private int m_maxLives = 3;
    private int m_lives;
    private int m_gatheredBonuses = 0;
    private Rigidbody m_rigidbody;

    // Cheats
    private bool m_invulnerable = false;

    private void Awake()
    {
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }

        m_character = gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_lives = m_maxLives;
        s_singleton = this;
    }

    public int gatheredBonuses
    {
        get { return m_gatheredBonuses; }
        set { m_gatheredBonuses = value; }
    }

    public int maxLives
    {
        get { return m_maxLives; }
    }

    public int lives
    {
        get { return m_lives; }
    }

    static public Protector protector
    {
        get { return s_singleton; }
    }

    private void ReduceLifeCount()
    {
        if (m_invulnerable || (m_lives <= 0)) return;

        m_lives--;

        if(m_lives <= 0)
        {
            KillProtector(true);
        }
        else
        {
            AudioManager.PlayOneShot(AudioManager.protectorLifeLostSound);
        }
    }

    static public void KillProtector(bool useLoseSequenceLenght)
    {
        s_singleton.m_character.Die();
        GameManager.StartLoseSequence(useLoseSequenceLenght);
    }

    static public void ReviveProtector()
    {
        s_singleton.m_character.Revive();
    }

    static public void ReduceProtectorLifeCount()
    {
        s_singleton.ReduceLifeCount();
    }

    new public Rigidbody rigidbody
    {
        get { return m_rigidbody; }
    }

    public bool invulnerable
    {
        get { return m_invulnerable; }
        set { m_invulnerable = value; }
    }

    public bool grounded
    {
        get { return m_character.grounded; }
    }

    public bool blocked
    {
        get { return m_character.blocked; }
        set { m_character.blocked = value; }
    }

    public void UpdateControls(GamePadState gamePadState)
    {
        if (!m_Jump)
        {
            m_Jump = GameManager.GetADown();
        }

        float h = gamePadState.ThumbSticks.Left.X;
        float v = gamePadState.ThumbSticks.Left.Y;
        if (v < 0) v = 0;

        bool dance = (gamePadState.Triggers.Left == 1) && (gamePadState.Triggers.Right == 1);

        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }

        m_character.Move(m_Move, false, m_Jump, dance);
        m_Jump = false;
    }
}
