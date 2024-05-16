using UnityEngine;
using Unity.Networking.Transport;

public class ClientBehaviour : MonoBehaviour
{
    NetworkDriver m_Driver;
    NetworkConnection m_Connection;

    string m_Name;
    string m_ip;

    public void UpdateName(string _name)
    {
        m_Name = _name;
    }
    public void UpdateIP(string _ip)
    {
        m_ip = _ip;
    }

    void Start()
    {
        m_Driver = NetworkDriver.Create();

        
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

                //uint value = 1;
                m_Driver.BeginSend(m_Connection, out var writer);
                writer.WriteFixedString32(m_Name);
                m_Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                uint value = stream.ReadUInt();
                Debug.Log($"Got the value {value} back from the server.");

                m_Connection.Disconnect(m_Driver);
                m_Connection = default;
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server.");
                m_Connection = default;
            }
        }

    }
}
