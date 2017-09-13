using UnityEngine;
using System.Collections;

public class Bat : DestructibleTrap
{
    public enum BatMode
    {
        Idle,
        Attack,
        ReturnToCage
    }
    static private float s_targetChangeThreshold = 0.01f;

    [SerializeField] private BatMode m_mode = BatMode.Idle;
    private BatTrap m_manager;
    private Vector3 m_target;
    private float m_autoRetargetTime;

    [SerializeField] private AudioClip m_onAttackSound;
    private AudioSource m_attackAudioSource;

	override protected void Awake ()
	{
	    m_autoRetargetTime = Random.Range(2f, 4f);

        m_attackAudioSource = GetComponent<AudioSource>();
        m_attackAudioSource.pitch = Random.Range(0.95f, 1.05f);
	} 

	private void Update ()
	{
	    switch(m_mode)
        {
            case BatMode.Idle:
                UpdateIdle();
                break;

            case BatMode.Attack:
                UpdateAttackProtector();
                break;

            case BatMode.ReturnToCage:
                UpdateReturnToCage();
                break;
        }

        if(!m_attackAudioSource.isPlaying && (m_mode == BatMode.Attack))
        {
            m_attackAudioSource.Play();
            m_attackAudioSource.PlayOneShot(m_onAttackSound, 2);
        }
        else if (m_attackAudioSource.isPlaying && (m_mode != BatMode.Attack))
        {
            m_attackAudioSource.Stop();
        }
	}

    private void UpdateIdle()
    {
        if (m_manager == null) return;

        Vector3 localBatPosition = transform.localPosition;
        float speedMul = m_manager.CageSpeedMultiplier(localBatPosition);

        if (!m_manager.CageContains(localBatPosition))
        {
            LookAtCagePoint(m_manager.GetRandomPositionInCage());
            m_target = m_manager.GetRandomTargetForBat(transform);
        }
        else if (NeedsNewTarget() || (m_autoRetargetTime <= 0))
        {
            m_target = m_manager.GetRandomTargetForBat(transform);
            m_autoRetargetTime = Random.Range(2f, 4f);
        }
        m_autoRetargetTime -= Time.deltaTime;

        Quaternion rotationToTarget = Quaternion.FromToRotation(transform.forward, m_target - localBatPosition);

        Vector3 normal = Vector3.Cross(transform.forward, m_target - localBatPosition);
        float angle, speed;
        Vector3 angleAxis;

        rotationToTarget.ToAngleAxis(out angle, out angleAxis);

        if (angle > m_manager.batAngularIdleSpeed)
        {
            angle = m_manager.batAngularIdleSpeed;
            //speed = 0.8f;
            speed = 1f;
        }
        else
        {
            //speed = 1f - 0.2f * angle / (m_batAngularSpeed);
            speed = 0.5f + 0.5f * angle / (m_manager.batAngularIdleSpeed);
        }

        speed *= m_manager.batIdleSpeed * speedMul;

        MoveForward(speed);
        transform.Rotate(normal, angle * Time.deltaTime);
    }

    private void MoveForward(float speed)
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void LookAtCagePoint(Vector3 localCagePoint)
    {
        transform.LookAt(m_manager.transform.TransformPoint(localCagePoint));
    }

    private bool NeedsNewTarget()
    {
        Vector3 delta = transform.localPosition - target;

        return delta.magnitude < s_targetChangeThreshold;
    }

    private void UpdateAttackProtector()
    {
        transform.LookAt(CameraHeadPivot.cameraHeadPivot.transform);

        transform.Translate(transform.forward * m_manager.batAttackSpeed * Time.deltaTime, Space.World);
    }

    protected override void OnHitProtector(Collision collision)
    {
        base.OnHitProtector(collision);

        Destroy(collision);

        m_manager.DeactivateBats();
    }

    private void UpdateReturnToCage()
    {
        if(m_manager.CageContains(transform.localPosition))
        {
            m_mode = BatMode.Idle;
            Activate();
            return;
        }

        MoveForward(m_manager.batAttackSpeed);
    }

    public void ReturnToCage()
    {
        m_mode = BatMode.ReturnToCage;
        m_target = m_manager.GetRandomPositionInCage();
        LookAtCagePoint(m_target);
    }

    public Vector3 target
    {
        get { return m_target; }
        set { m_target = value; }
    }

    public BatMode mode
    {
        get { return m_mode; }
        set { m_mode = value; }
    }

    public BatTrap manager
    {
        get { return m_manager; }
        set { m_manager = value; }
    }
}
