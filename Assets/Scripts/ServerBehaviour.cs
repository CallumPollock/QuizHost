using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using System.Net;
using System;

public class ServerBehaviour : MonoBehaviour
{

    NetworkDriver m_Driver;
    NativeList<NetworkConnection> m_Connections;
    public static Action<Player> PlayerConnected;

    public void StartServer()
    {
        m_Driver = NetworkDriver.Create();
        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        var endpoint = NetworkEndpoint.AnyIpv4.WithPort(7777);
        if (m_Driver.Bind(endpoint) != 0)
        {
            Debug.LogError("Failed to bind to port 7777.");
            return;
        }
        m_Driver.Listen();

        Debug.Log(string.Format("Started server on {0}", Dns.GetHostName()));
    }

    void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }

    public void SendMessageToClients(string _msg)
    {
        for (int i = 0; i < m_Connections.Length; i++)
        {
            MessageClient(m_Connections[i], _msg);
        }
    }

    void MessageClient(NetworkConnection client, string _msg)
    {
        DataStreamWriter writer;

        m_Driver.BeginSend(client, out writer);
        writer.WriteFixedString128(_msg);
        m_Driver.EndSend(writer);
    }

    void Update()
    {
        if(!m_Driver.IsCreated) { return; }

        m_Driver.ScheduleUpdate().Complete();

        // Clean up connections.
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                i--;
            }
        }

        // Accept new connections.
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default)
        {
            m_Connections.Add(c);
            Debug.Log("Accepted a connection.");
            MessageClient(c, "Lobby");
        }

        for (int i = 0; i < m_Connections.Length; i++)
        {
            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    byte messageType = stream.ReadByte();

                    switch(messageType)
                    {
                        //Client connection
                        case 1:
                            Debug.Log("Client has connected");
                            if(stream.ReadByte() != 0)
                            {
                                Player newPlayer = new Player();
                                newPlayer.name = stream.ReadFixedString32();
                                newPlayer.score = 0;
                                PlayerConnected?.Invoke(newPlayer);
                            }
                            else
                            {
                                Debug.Log("Client disocnnected");
                            }
                            
                            break;

                        //Load scene
                        case 2:
                            Debug.LogError("Cannot load scene on a server, this message should be for clients.");
                            break;

                        //Client answer
                        case 3:
                            Debug.Log("Client has answered");
                            break;
                        //Message not recognised, throw error and ignore
                        default:
                            Debug.LogError(String.Format("Message type ({0}) not recognised, it has been ignored.", messageType));
                            break;
                    }

                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from the server.");
                    m_Connections[i] = default;
                    break;
                }

            }

        }

    }
}