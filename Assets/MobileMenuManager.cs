using System;
using TMPro;
using UnityEngine;

public class MobileMenuManager : MonoBehaviour
{
    [SerializeField] GameObject chooseConnectionMethod;
    [SerializeField] GameObject phoneQRCamera;
    [SerializeField] GameObject ipSubmission;
    [SerializeField] GameObject nameEntry;
    [SerializeField] TextMeshProUGUI serverInfoTMP;
    [SerializeField] TMP_Text nameTextField;

    private void Start()
    {
        NavigateMenu(0);
        ClientBehaviour.ServerInfo += UpdateConnectedServerInfo;

        if(PlayerPrefs.HasKey("PlayerName"))
        {
            nameTextField.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void UpdateNamePlayerPref(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    void UpdateConnectedServerInfo(string serverInfo)
    {
        NavigateMenu(3);
        serverInfoTMP.text = String.Format("Connected to host:\n{0}", serverInfo);
        
    }

    public void NavigateMenu(int menu)
    {
        chooseConnectionMethod.SetActive(false);
        phoneQRCamera.SetActive(false);
        ipSubmission.SetActive(false);
        nameEntry.SetActive(false);

        switch(menu)
        {//0 = main; 1 = qr code; 2 = manual; 3 = name selection
            case 0:
                chooseConnectionMethod.SetActive(true);
                break;
            case 1:
                phoneQRCamera.SetActive(true);
                break;
            case 2:
                ipSubmission.SetActive(true);
                break;
            case 3:
                nameEntry.SetActive(true);
                break;
            default:
                Debug.LogError("No such menu exists");
                break;

        }
    }
}
