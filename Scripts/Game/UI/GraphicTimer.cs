using UnityEngine;
using UnityEngine.UI;

public class GraphicTimer : MonoBehaviour
{
    private Text m_display;

    private float m_time = 0;
    private bool m_paused = false;

    private int m_mins = 0;
    private int m_seconds = 0;

	private void Awake ()
    {
        m_display = GetComponent<Text>();
	}
	
	private void Update ()
    {
	    if(!m_paused)
        {
            m_time += Time.deltaTime;
            UpdateTimeValues();
            m_display.text = TimeToString();
        }
    }

    private string TimeToString()
    {
        string result = "";

        if(mins < 10)
        {
            result += "0" + mins.ToString() + ':';
        }
        else
        {
            result += mins.ToString() + ':';
        }

        if(seconds < 10)
        {
            result += "0" + seconds.ToString();
        }
        else
        {
            result += seconds.ToString();
        }

        return result;
    }

    private void UpdateTimeValues()
    {
        m_mins = (int)(m_time / 60);
        m_seconds = (int)m_time - m_mins * 60;
    }

    public int mins
    {
        get { return m_mins; }
    }

    public int seconds
    {
        get { return m_seconds; }
    }

    public float time
    {
        get { return m_time; }
    }

    public string formatedTime
    {
        get { return TimeToString(); }
    }

public bool paused
    {
        get { return m_paused; }
        set { m_paused = value; }
    }
}
