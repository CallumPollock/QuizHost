using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System;
using UnityEngine.SceneManagement;

public class ClientBehaviour : MonoBehaviour
{
    NetworkDriver m_Driver;
    NetworkConnection m_Connection;

    Player player;

    string m_ip = "127.0.0.1";
    public static Action<Scenes> LoadScene;

    KeyboardHandler m_KeyboardHandler;

    public void UpdateName(string _name)
    {
        player.name = _name;
    }
    public void UpdateIP(string _ip)
    {
        m_ip = _ip;
    }
    public void OnClickConnect()
    {
        ConnectToServer(m_ip);
    }


    void Start()
    {
        m_Driver = NetworkDriver.Create();
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;

        QRCodeScanner.QRCodeRead += ConnectToServer;
    }

    void CreateAnswerMessage(byte[] ans_bytes)
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_KeyboardHandler = FindAnyObjectByType<KeyboardHandler>();
        m_KeyboardHandler.OnKeyPress += SendMessageToServer;
    }

    public void SendMessageToServer(NetworkMessage netMsg)
    {
        DataStreamWriter writer;
        m_Driver.BeginSend(m_Connection, out writer);
        netMsg.Serialize(ref writer);
        m_Driver.EndSend(writer);
    }

    void ConnectToServer(string ip)
    {
        //var endpoint = NetworkEndpoint.LoopbackIpv4.WithPort(7777);
        var endpoint = NetworkEndpoint.Parse(ip, 7777,NetworkFamily.Ipv4);

        m_Connection = m_Driver.Connect(endpoint);
    }

    void OnDestroy()
    {
        m_Driver.Dispose();
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }

        Unity.Collections.DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server.");

                Net_ClientConnection clientConnection = new Net_ClientConnection(true, player);
                SendMessageToServer(clientConnection);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                byte messageType = stream.ReadByte();

                switch (messageType)
                {
                    //Client connection
                    case 1:
                        Debug.LogError("This message is for the server, client will ignore it.");
                        break;

                    //Load scene
                    case 2:
                        LoadScene?.Invoke((Scenes)stream.ReadByte());
                        break;

                    //Client answer
                    case 3:
                        Byte ans = stream.ReadByte();
                        Debug.Log(String.Format("Server has confirmed answer with {0}", ans));
                        m_KeyboardHandler.ConfirmAnswer(ans);
                        break;
                    //Message not recognised, throw error and ignore
                    default:
                        Debug.LogError(String.Format("Message type ({0}) not recognised, it has been ignored.", messageType));
                        break;
                }
                
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server.");
                m_Connection = default;
                LoadScene?.Invoke(0);
            }
        }

    }
}
