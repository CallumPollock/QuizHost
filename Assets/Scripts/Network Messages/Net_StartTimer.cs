using Unity.Collections;

public class Net_StartTimer : NetworkMessage
{
    int time;

    public Net_StartTimer(int _time)
    {
        Type = MessageType.StartTimer;
        time = _time;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteInt(time);
    }

}
