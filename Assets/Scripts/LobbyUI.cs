using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUI : MonoBehaviour
{

    [SerializeField] GameObject lobbyPlayerPrefab;
    [SerializeField] Transform lobbyTransform;

    [SerializeField] GameObject serverStartedTools;
    [SerializeField] GameObject startServerButton;

    private void Start()
    {
        ServerBehaviour.PlayerConnected += CreateNewPlayer;
        serverStartedTools.SetActive(false);
        startServerButton.SetActive(true);
    }

    void CreateNewPlayer(Player player)
    {
        GameObject newLobbyPlayer = Instantiate(lobbyPlayerPrefab, lobbyTransform);

        newLobbyPlayer.GetComponent<TextMeshProUGUI>().text = String.Format("<b>{0}</b> [{1}] - Connected", player.name, player.score);
    }

    public void ServerStarted()
    {
        serverStartedTools.SetActive(true);
        startServerButton.SetActive(false);
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
