using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private TMP_Text m_timerText;
    private Image m_backgroundSlider;

    float m_time;
    float m_maxTime;



    private void Awake()
    {
        m_backgroundSlider = GetComponent<Image>();
        m_timerText = GetComponentInChildren<TMP_Text>();
        ClientBehaviour.StartTimer += StartTimer;
        m_backgroundSlider.fillAmount = 0f;
        m_timerText.text = null;

    }

    public void StartTimer(int seconds)
    {
        m_maxTime = seconds;
        m_time = (float)seconds;
    }

    private void Update()
    {
        if(m_time <=0)
        {
            m_backgroundSlider.fillAmount = 0f;
            m_timerText.text = null;
            return;
        }

        m_time = Mathf.Max(0f, m_time-Time.deltaTime);
        m_timerText.text = m_time.ToString("0");
        m_backgroundSlider.fillAmount = 1f - (m_time / m_maxTime);
    }

}
