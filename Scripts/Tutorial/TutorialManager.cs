using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private Mummy m_mummy;
    [SerializeField] private AudioSource m_horusEye;
    private bool m_mummyTouchedPlayer = false;
    private GameObject m_fairyPowerSlider;

    private void Start()
    {
        // Casser UI 2
        Fairy.fairy.Disable();


        FindObjectOfType<GraphicTimer>().gameObject.SetActive(false);
        m_fairyPowerSlider = FindObjectOfType<FairyPowerSlider>().gameObject;
        m_fairyPowerSlider.SetActive(false);
    }

	private void Update ()
	{
        if (!m_mummy && !m_mummyTouchedPlayer)
        {
            m_mummyTouchedPlayer = true;
            StartCoroutine(PresentHorusEyeCoroutine());
        }
    }

    private IEnumerator PresentHorusEyeCoroutine()
    {
        Protector.protector.blocked = true;
        m_horusEye.gameObject.SetActive(true);

        Vector3 posSource = m_horusEye.transform.position;
        Quaternion rotSource = m_horusEye.transform.rotation;
        Vector3 scaleSource = m_horusEye.transform.localScale;

        yield return new WaitForSeconds(m_horusEye.clip.length);

        float time = 0;
        while (time < 1)
        {
            m_horusEye.transform.position = Vector3.Lerp(posSource, Fairy.fairy.transform.position, time);
            m_horusEye.transform.rotation = Quaternion.Slerp(rotSource, Fairy.fairy.transform.rotation, time);
            m_horusEye.transform.localScale = Vector3.Lerp(scaleSource, Fairy.fairy.transform.lossyScale, time);

            time += Time.deltaTime;
            yield return null;
        }

        m_horusEye.gameObject.SetActive(false);
        Fairy.fairy.Enable();
        Protector.protector.blocked = false;
        m_fairyPowerSlider.SetActive(true);
    }
}
