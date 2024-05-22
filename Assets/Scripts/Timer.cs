using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Image m_clock;
    private TMP_Text m_timerText;

    private AudioSource countdown;

    float m_time;
    float m_maxTime;



    private void Awake()
    {
        m_clock = GetComponent<Image>();
        m_timerText = GetComponentInChildren<TMP_Text>();
        ClientBehaviour.StartTimer += StartTimer;
        m_clock.fillAmount = 0f;
        m_timerText.text = null;

        countdown = GetComponent<AudioSource>();
    }

    public void StartTimer(int seconds)
    {
        m_maxTime = seconds;
        m_clock.fillAmount = 1;
        m_time = (float)seconds;
        countdown.Stop();
    }

    private void Update()
    {
        if(m_time <=0)
        {
            m_clock.fillAmount = 0f;
            m_timerText.text = null;
            return;
        }

        if(!countdown.isPlaying && m_time <= 30)
        {
            countdown.Play();
            countdown.time = 30f - m_time;
        }

        m_time = Mathf.Max(0f, m_time-Time.deltaTime);
        m_timerText.text = m_time.ToString("0");
        m_clock.fillAmount = 1f / ((m_maxTime / m_time)/10f);
    }

}
