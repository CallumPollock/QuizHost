using System;
using Unity.Collections;

[Serializable]
public enum Scenes
{
    Lobby = 1,
    Keypad = 2,
}

public class Net_LoadSceneMessage : NetworkMessage
{
    Scenes scene;

    public Net_LoadSceneMessage(Scenes _scene)
    {
        Type = MessageType.LoadScene;
        scene = _scene;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Type);
        writer.WriteByte((byte)scene);
    }
}
