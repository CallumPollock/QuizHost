using Unity.Collections;

public class Net_ServerInfo : NetworkMessage
{
    FixedString32Bytes hostName;

    public Net_ServerInfo(FixedString32Bytes _hostName)
    {
        Type = MessageType.ServerInfo;
        hostName = _hostName;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteFixedString32(hostName);
    }

}
