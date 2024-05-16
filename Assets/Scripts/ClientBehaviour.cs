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
    public static Action<FixedString128Bytes> messageReceived;

    KeyboardHandler m_KeyboardHandler;

    public void UpdateName(string _name)
    {
        player.name = _name;
    }
    public void UpdateIP(string _ip)
    {
        m_ip = _ip;
    }
    

    void Start()
    {
        m_Driver = NetworkDriver.Create();
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_KeyboardHandler = FindAnyObjectByType<KeyboardHandler>();
        //m_KeyboardHandler.OnKeyPress += SendMessageToServer;
    }

    public void SendMessageToServer(NetworkMessage netMsg)
    {
        DataStreamWriter writer;
        m_Driver.BeginSend(m_Connection, out writer);
        netMsg.Serialize(ref writer);
        m_Driver.EndSend(writer);
    }

    public void ConnectToServer()
    {
        //var endpoint = NetworkEndpoint.LoopbackIpv4.WithPort(7777);
        var endpoint = NetworkEndpoint.Parse(m_ip, 7777,NetworkFamily.Ipv4);

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
                FixedString128Bytes msg = stream.ReadFixedString128();
                Debug.Log($"Got the message {msg} back from the server.");
                messageReceived?.Invoke(msg);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server.");
                m_Connection = default;
            }
        }

    }
}
