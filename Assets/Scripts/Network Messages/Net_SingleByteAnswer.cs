using Unity.Collections;

public class Net_SingleByteAnswer : Net_Answer
{
    byte answer;

    public Net_SingleByteAnswer(byte _answer)
    {
        Type = MessageType.ClientAnswer;
        answer = _answer;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteByte(answer);
    }

}
