using Unity.Collections;

public class Net_ClientConnection : NetworkMessage
{
    bool clientJoined;

    public Net_ClientConnection(bool _clientJoined)
    {
        Type = MessageType.ClientConnection;
        clientJoined = _clientJoined;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteByte((byte)(clientJoined ? 1 : 0));
    }

}
