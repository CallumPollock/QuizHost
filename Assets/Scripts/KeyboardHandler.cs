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
    public Action<char> OnKeyPress;

    // Start is called before the first frame update
    void Start()
    {
        SetUpKeyboard();
    }

    void SetUpKeyboard()
    {
        for(int i = 65; i < 91; i++)
        {
            Char c = Convert.ToChar(i);

            GameObject newKey = Instantiate(m_keyPrefab);
            newKey.transform.SetParent(m_gridTransorm, false);
            newKey.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();
            newKey.GetComponent<Button>().onClick.AddListener(() => OnKeyPress(c));
        }
    }
}
