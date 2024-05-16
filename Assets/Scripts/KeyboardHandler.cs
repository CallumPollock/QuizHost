using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    [SerializeField] Transform m_gridTransorm;
    [SerializeField] GameObject m_keyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SetUpKeyboard();
    }

    void SetUpKeyboard()
    {
        for(int i = 0; i < 26; i++)
        {
            GameObject newKey = Instantiate(m_keyPrefab);
            newKey.transform.SetParent(m_gridTransorm, false);
            newKey.GetComponentInChildren<TextMeshProUGUI>().text = Convert.ToChar(i+65).ToString();
        }
    }
}
