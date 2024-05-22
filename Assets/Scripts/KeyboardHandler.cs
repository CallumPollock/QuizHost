using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardHandler : MonoBehaviour
{
    [SerializeField] Transform m_gridTransorm;
    [SerializeField] GameObject m_keyPrefab;

    public Action<Net_CharAnswer> OnKeyPress;


    private Dictionary<int, Button> m_keys = new Dictionary<int, Button>();

    // Start is called before the first frame update
    void Start()
    {
        SetUpKeyboard();
    }

    void SetUpKeyboard()
    {
        m_keys.Clear();
        for(int i = 65; i < 91; i++)
        {
            Char c = Convert.ToChar(i);

            GameObject newKey = Instantiate(m_keyPrefab);
            newKey.transform.SetParent(m_gridTransorm, false);
            newKey.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();
            newKey.GetComponent<Button>().onClick.AddListener(() => CreateAnswerMessageAndSend(c));
            m_keys.Add(i, newKey.GetComponent<Button>());
        }
    }

    void CreateAnswerMessageAndSend(char c)
    {
        Net_CharAnswer message = new Net_CharAnswer(c);
        OnKeyPress(message);

    }
    
    public void ConfirmAnswer(Byte answer)
    {
        for (int i = 65; i < 91; i++)
        {
            if (m_keys[i].GetComponentInChildren<TextMeshProUGUI>().Equals((char)answer))
            {
                m_keys[i].interactable = true;
            }
            else
            {
                m_keys[i].interactable=false;
            }
        }
    }

    public void ResetKeypad()
    {
        for (int i = 65; i < 91; i++)
        {
            m_keys[i].interactable = true;
        }
    }
}
