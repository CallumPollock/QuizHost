using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMenuManager : MonoBehaviour
{
    [SerializeField] GameObject chooseConnectionMethod;
    [SerializeField] GameObject phoneQRCamera;
    [SerializeField] GameObject ipSubmission;

    private void Start()
    {
        NavigateMenu(0);
    }

    public void NavigateMenu(int menu)
    {
        chooseConnectionMethod.SetActive(false);
        phoneQRCamera.SetActive(false);
        ipSubmission.SetActive(false);

        switch(menu)
        {//0 = main; 1 = qr code; 2 = manual
            case 0:
                chooseConnectionMethod.SetActive(true);
                break;
            case 1:
                phoneQRCamera.SetActive(true);
                break;
            case 2:
                ipSubmission.SetActive(true);
                break;
            default:
                Debug.LogError("No such menu exists");
                break;

        }
    }
}
