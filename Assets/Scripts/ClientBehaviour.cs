using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System;
using UnityEngine.SceneManagement;

public class ClientBehaviour : MonoBehaviour
{
    NetworkDriver m_Driver;
    NetworkConnection m_Connection;

    String playerName;

    string m_ip = "127.0.0.1";
    public static Action<Scenes> LoadScene;
    public static Action<String> ServerInfo;
    public static Action<int> StartTimer;

    KeyboardHandler m_KeyboardHandler;

    public void UpdateName(string _name)
    {
        playerName = _name;
    }
    public void UpdateIP(string _ip)
    {
        m_ip = _ip;
    }
    public void OnClickConnect()
    {
        ConnectToServer(m_ip);
    }

    public void JoinSession()
    {
        SendMessageToServer(new Net_JoinSession(true, playerName));
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

                Net_ClientConnection clientConnection = new Net_ClientConnection(true);
                SendMessageToServer(clientConnection);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                MessageType messageType = (MessageType)Enum.Parse(typeof(MessageType), stream.ReadByte().ToString());

                switch (messageType)
                {
                    //Load scene
                    case MessageType.LoadScene:
                        LoadScene?.Invoke((Scenes)stream.ReadByte());
                        break;

                    //Client answer
                    case MessageType.ClientAnswer:
                        Byte ans = stream.ReadByte();
                        Debug.Log(String.Format("Server has confirmed answer with {0}", ans));
                        m_KeyboardHandler.ConfirmAnswer(ans);
                        break;

                    case MessageType.ServerInfo:
                        ServerInfo?.Invoke(stream.ReadFixedString32().ToString());
                        break;

                    case MessageType.StartTimer:
                        StartTimer?.Invoke(stream.ReadInt());
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
