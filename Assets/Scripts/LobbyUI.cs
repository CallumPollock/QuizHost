using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{

    [SerializeField] GameObject lobbyPlayerPrefab;
    [SerializeField] Transform lobbyTransform;

    [SerializeField] GameObject serverStartedTools;
    [SerializeField] GameObject startServerButton;
    [SerializeField] GameObject keyPadArea;

    [SerializeField] TextMeshProUGUI playersList;

    [SerializeField] Slider m_timerSlider;
    [SerializeField] TMP_InputField m_timerText;

    private void Start()
    {
        //ServerBehaviour.PlayerConnected += CreateNewPlayer;
        ServerBehaviour.UpdatePlayerList += UpdatePlayerList;
        serverStartedTools.SetActive(false);
        startServerButton.SetActive(true);
        keyPadArea.SetActive(false);

        m_timerSlider.onValueChanged.AddListener(delegate { UpdateTimerText(m_timerSlider.value.ToString()); });
        m_timerText.onValueChanged.AddListener(delegate { UpdateTimerSlider(int.Parse(m_timerText.text)); });
    }

    void UpdatePlayerList(Player[] players)
    {
        playersList.text = null;
        foreach(Player player in players)
        {
            playersList.text += String.Format("<b>{0}</b> [{1}]\n", player.name, player.score);
        }
    }

    public void ServerStarted()
    {
        serverStartedTools.SetActive(true);
        startServerButton.SetActive(false);
        keyPadArea.SetActive(false);
    }

    public void KeypadArea()
    {
        keyPadArea.SetActive(true);
        serverStartedTools.SetActive(false);
        startServerButton.SetActive(false);
    }

    private void UpdateTimerSlider(int value)
    {
        m_timerSlider.value = value;
    }

    private void UpdateTimerText(string text)
    {
        m_timerText.text = text;
    }

    // UXML template for list entries
    /*[SerializeField] VisualTreeAsset m_ListEntryTemplate;

    ListView m_playerList;

    public void InitializePlayerList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        // Store a reference to the template for the list entries
        m_ListEntryTemplate = listElementTemplate;

        m_playerList.makeItem = () =>
        {
            var newListEntry = m_ListEntryTemplate.Instantiate();
            return newListEntry;
        };
        m_playerList.itemsSource = new List<VisualElement>();
    }

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        InitializePlayerList(uiDocument.rootVisualElement, m_ListEntryTemplate);
    }*/
}
