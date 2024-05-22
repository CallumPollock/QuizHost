using Unity.Collections;

public class Player 
{
    public FixedString32Bytes name;
    public int score;

    public Player(FixedString32Bytes _name)
    {
        name = _name;
        score = 0;
    }
}
