using Unity.Collections;

public class Net_RevealQuestion : NetworkMessage
{
    FixedString128Bytes question;
    FixedString64Bytes answer;

    public Net_RevealQuestion(FixedString128Bytes _question, FixedString64Bytes _answer)
    {
        Type = MessageType.RevealQuestion;
        question = _question;
        answer = _answer;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteFixedString128(question);
        writer.WriteFixedString64(answer);
    }

}
