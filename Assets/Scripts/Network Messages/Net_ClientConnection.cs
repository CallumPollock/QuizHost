using Unity.Collections;

public class Net_ClientConnection : NetworkMessage
{
    bool clientJoined;
    byte reason;

    public Net_ClientConnection(bool _clientJoined, byte _reason)
    {
        Type = MessageType.ClientConnection;
        clientJoined = _clientJoined;
        reason = _reason;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteByte((byte)(clientJoined ? 1 : 0));
        writer.WriteByte(reason);
    }

}
