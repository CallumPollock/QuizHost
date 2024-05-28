using Unity.Collections;
using Unity.Networking.Transport;

public class Player 
{
    public FixedString32Bytes name;
    public int score;
    public NetworkConnection connection;

    public Player(FixedString32Bytes _name, NetworkConnection _connection)
    {
        name = _name;
        connection = _connection;
        score = 0;
    }
}
