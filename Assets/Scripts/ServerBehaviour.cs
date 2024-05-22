using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class ServerBehaviour : MonoBehaviour
{

    NetworkDriver m_Driver;
    NativeList<NetworkConnection> m_Connections;
    //public static Action<Player> PlayerConnected;
    public static Action<Player[]> UpdatePlayerList;

    private Dictionary<NetworkConnection, Player> m_Players = new Dictionary<NetworkConnection, Player>();

    private Dictionary<Player, Byte> m_playerAnswers = new Dictionary<Player, byte>();

    [SerializeField] Slider m_timerValue;
    [SerializeField] TMP_Text m_questionText, m_answerText;

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

    public void SendClientsToScene(string scene)
    {
        SendMessageToAllClients(new Net_LoadSceneMessage((Scenes)Enum.Parse(typeof(Scenes), scene)));
    }

    public void StartTimer()
    {
        SendMessageToAllClients(new Net_StartTimer((int)m_timerValue.value));
    }

    void SendMessageToAllClients(NetworkMessage _msg)
    {
        for (int i = 0; i < m_Connections.Length; i++)
        {
            MessageClient(m_Connections[i], _msg);
        }
    }

    void MessageClient(NetworkConnection client, NetworkMessage _msg)
    {
        DataStreamWriter writer;

        m_Driver.BeginSend(client, out writer);
        _msg.Serialize(ref writer);
        m_Driver.EndSend(writer);
    }

    public void RevealQuestion()
    {
        SendMessageToAllClients(new Net_RevealQuestion(m_questionText.text, ""));
    }

    public void RevealAnswer()
    {
        SendMessageToAllClients(new Net_RevealQuestion(m_questionText.text, "Answer: " +m_answerText.text));

        CheckAnswers(((byte)m_answerText.text.ToCharArray()[0]));
    }

    private void CheckAnswers(byte _answer)
    {
        foreach(KeyValuePair<Player, byte> _playerAnswer in  m_playerAnswers)
        {
            Debug.Log(String.Format("{0} answered {1}, the correct answer was {2}", _playerAnswer.Key.name, (char)_playerAnswer.Value, (char)_answer));

            if(_playerAnswer.Value == _answer)
            {
                _playerAnswer.Key.score += 1;
            }
            
        }
        m_playerAnswers.Clear();
        UpdatePlayerList?.Invoke(m_Players.Values.ToArray());
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
            
        }

        for (int i = 0; i < m_Connections.Length; i++)
        {
            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    MessageType messageType = (MessageType)Enum.Parse(typeof(MessageType), stream.ReadByte().ToString());

                    switch(messageType)
                    {
                        //Client connection
                        case MessageType.ClientConnection:
                            
                            if(stream.ReadByte() != 0)
                            {
                                Debug.Log("Client has connected");
                                MessageClient(m_Connections[i], new Net_ServerInfo(Dns.GetHostName()));
                            }
                            else
                            {
                                Debug.Log("Client disocnnected");
                            }
                            
                            break;

                        //Client join session
                        case MessageType.JoinSession:
                            if (stream.ReadByte() != 0)
                            {
                                Player newPlayer = new Player(stream.ReadFixedString32());
                                //PlayerConnected?.Invoke(newPlayer);
                                m_Players.Add(m_Connections[i], newPlayer);
                                UpdatePlayerList?.Invoke(m_Players.Values.ToArray());
                                MessageClient(m_Connections[i], new Net_LoadSceneMessage(Scenes.Lobby));
                            }
                            else
                            {
                                Debug.Log("Client disocnnected");
                            }
                            break;

                        //Load scene
                        case MessageType.LoadScene:
                            Debug.LogError("Cannot load scene on a server, this message should be for clients.");
                            break;

                        //Client answer
                        case MessageType.ClientAnswer:
                            Byte ans = stream.ReadByte();
                            Debug.Log(String.Format("Client has answered with {0}", ans));
                            MessageClient(m_Connections[i], new Net_CharAnswer((char)ans));
                            m_playerAnswers.Add(m_Players[m_Connections[i]], ans);
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