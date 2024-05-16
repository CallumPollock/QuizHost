using Unity.Collections;

public class Net_ClientConnection : NetworkMessage
{
    bool clientJoined;
    Player player;

    public Net_ClientConnection(bool _clientJoined, Player _player)
    {
        Type = MessageType.ClientConnection;
        clientJoined = _clientJoined;
        player = _player;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Type);
        writer.WriteByte((byte)(clientJoined ? 1 : 0));
        writer.WriteFixedString32(player.name);
    }

}
