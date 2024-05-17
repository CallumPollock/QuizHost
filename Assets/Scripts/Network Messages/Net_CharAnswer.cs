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
        writer.WriteByte((byte)Type);
        writer.WriteByte((byte)answer);
    }

}
