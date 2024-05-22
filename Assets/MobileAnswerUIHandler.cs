using TMPro;
using UnityEngine;

public class MobileAnswerUIHandler : MonoBehaviour
{

    [SerializeField] TMP_Text m_questionText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClientBehaviour.RevealQuestion += UpdateQuestionText;
    }

    private void UpdateQuestionText(string _text)
    {
        m_questionText.text = _text;
    }
}
