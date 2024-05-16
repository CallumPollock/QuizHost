
using Unity.Collections;

public enum MessageType
{
    ClientConnection = 1,
    LoadScene = 2,
    ClientAnswer = 3,
}

public abstract class NetworkMessage 
{
    public MessageType Type { set; get; }

    public abstract void Serialize(ref DataStreamWriter writer);
}
