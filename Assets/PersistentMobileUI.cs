using TMPro;
using UnityEngine;

public class PersistentMobileUI : MonoBehaviour
{

    [SerializeField] TMP_Text m_teamName, m_teamScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ClientBehaviour.UpdatePlayerScore += UpdateScore;
        ClientBehaviour.UpdateTeamName += UpdateName;
    }

    private void UpdateScore(int _score)
    {
        m_teamScore.text = string.Format("Score: {0}", _score);
    }

    private void UpdateName(string _name)
    {
        m_teamName.text = _name;
    }
}
