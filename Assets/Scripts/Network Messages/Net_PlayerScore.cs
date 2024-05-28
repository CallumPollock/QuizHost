using Unity.Collections;

public class Net_PlayerScore : NetworkMessage
{
    int score;

    public Net_PlayerScore(int _score)
    {
        Type = MessageType.PlayerScore;
        score = _score;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        base.Serialize(ref writer);
        writer.WriteInt(score);
    }

}
