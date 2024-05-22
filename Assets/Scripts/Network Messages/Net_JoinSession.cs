using Unity.Collections;

public class Net_JoinSession : NetworkMessage
{
    bool clientJoined;
    FixedString32Bytes playerName;

    public Net_JoinSession(bool _clientJoined, FixedString32Bytes _playerName)
    {
        Type = MessageType.JoinSession;
        clientJoined = _clientJoined;
        playerName = _playerName;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteByte((byte)(clientJoined ? 1 : 0));
        writer.WriteFixedString32(playerName);
    }

}
