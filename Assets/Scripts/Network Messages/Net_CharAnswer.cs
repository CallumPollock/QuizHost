using Unity.Collections;

public class Net_CharAnswer : Net_Answer
{
    char answer;

    public Net_CharAnswer(char _answer)
    {
        Type = MessageType.ClientAnswer;
        answer = _answer;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteByte((byte)answer);
    }

}
