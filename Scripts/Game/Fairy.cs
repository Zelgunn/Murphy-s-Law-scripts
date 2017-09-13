using UnityEngine;
using System.Collections;

public class Fairy : MonoBehaviour
{
    static private Fairy s_singleton;
    
    [Header("Transform")]
    [SerializeField] private float m_fairyRangeDivider = 5;
    [SerializeField] private Transform m_pivot;
    [Header("Power")]
    [SerializeField] private float m_power = 1;
    [SerializeField] private float m_powerRecoverTime = 5f;
    [SerializeField] private float m_powerDuration = 0.5f;
    [SerializeField] private FairyPowerSound m_powerSoundPrefab;
    private float m_powerRemainingTime = 0;
    private Transform m_falconTransform;

    private float m_strength = 1;
    private int m_destroyedElements = 0;

    private Vector3 m_previousPosition;
    private Vector3 m_velocity;

    private Collider m_collider;
    private Renderer m_renderer;
    private bool m_disabled = false;

    private void Awake()
    {
        s_singleton = this;
        m_collider = GetComponent<Collider>();
        m_renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        m_falconTransform = GameManager.falconGodObject;
    }

	private void Update ()
    {
        UpdateTransform();
        if (m_disabled) return;

        Vector3 resistance = UpdateStrength();

        UpdatePower();

        if(powerActivated)
        {
            FalconUnity.setForceField(0, Vector3.up * 50);
        }
        else
        {
            FalconUnity.setForceField(0, resistance);
        }
    }

    private void UpdateTransform()
    {
        transform.position = m_pivot.position + m_pivot.rotation * m_falconTransform.localPosition / m_fairyRangeDivider;

        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.right * 90, Space.Self);
    }

    private Vector3 UpdateStrength()
    {
        Vector3 resistance = GameManager.falconFairyMaxResistance * (1 - m_strength) * (1 - m_strength);

        if (m_strength > 0) m_strength -= Time.deltaTime / 30;

        return resistance;
    }

    public void Disable()
    {
        m_collider.enabled = false;
        m_renderer.enabled = false;

        m_disabled = true;
    }

    public void Enable()
    {
        m_collider.enabled = true;
        m_renderer.enabled = true;

        m_disabled = false;
    }

    #region Power
    private void UpdatePower()
    {
        bool[] buttonsStates;
        FalconUnity.getFalconButtonStates(0, out buttonsStates);
        bool aButtonIsPressed = Input.GetKeyDown(KeyCode.Return);
        for (int i = 0; i < buttonsStates.Length; i++) aButtonIsPressed |= buttonsStates[i];
        if (aButtonIsPressed && !Protector.protector.grounded) UsePower();

        if (m_powerRemainingTime > 0)
        {
            if (!Protector.protector.grounded)
            {
                Rigidbody protectorRigidbody = Protector.protector.rigidbody;
                m_velocity = (Fairy.fairy.transform.position - m_previousPosition);

                m_velocity /= Time.deltaTime;
                m_velocity -= protectorRigidbody.velocity;
                m_velocity /= Mathf.Sqrt(m_velocity.magnitude);
                m_velocity *= 4;

                protectorRigidbody.AddForce(m_velocity, ForceMode.Impulse);
            }

            m_powerRemainingTime -= Time.deltaTime;
        }

        m_previousPosition = Fairy.fairy.transform.position;

        if (m_power < 1)
        {
            m_power += Time.deltaTime / m_powerRecoverTime;
            if (m_power > 1) m_power = 1;
        }
        else
        {
            if (m_power > 1) m_power = 1;
        }
    }

    public void UsePower()
    {
        if (!powerAvailable) return;

        FairyPowerSound fairyPowerSound = Instantiate(m_powerSoundPrefab);
        fairyPowerSound.PlaySound(m_powerDuration);

        m_powerRemainingTime = m_powerDuration;
        m_power -= 1;
    }

    public void Regenerate()
    {
        m_strength = 1;
        AudioManager.PlayOneShot(AudioManager.fairyPowerUpSound);
    }
    #endregion

    #region Accesseurs
    public float power
    {
        get { return m_power; }
    }

    public bool powerAvailable
    {
        get { return m_power >= 1; }
    }

    public bool powerActivated
    {
        get { return (m_powerRemainingTime > 0) && !Protector.protector.grounded; }
    }

    public float strength
    {
        get { return m_strength; }
    }

    public int destroyedElements
    {
        get { return m_destroyedElements; }
        set {  m_destroyedElements = value; }
    }

    public float velocity
    {
        get { return m_velocity.magnitude; }
    }

    static public Fairy fairy
    {
        get { return s_singleton; }
    }
    #endregion
}
