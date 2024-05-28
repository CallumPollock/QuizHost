
using Unity.Collections;

public enum MessageType
{
    ClientConnection = 1,
    LoadScene = 2,
    ClientAnswer = 3,
    JoinSession = 4,
    ServerInfo = 5,
    StartTimer = 6,
    RevealQuestion = 7,
    PlayerScore = 8,
}

public abstract class NetworkMessage 
{
    public MessageType Type { set; get; }

    public virtual void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Type);
    }
}
