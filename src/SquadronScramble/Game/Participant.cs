namespace SquadronScramble;

public class Participant
{
	private bool participating = false;

	private int currentControllerPosition;

	private int currentPlayer;

	public void SetParticipating(bool b)
	{
		participating = b;
	}

	public bool GetParticipating()
	{
		return participating;
	}

	public void SetCurrentPlayer(int i)
	{
		currentPlayer = i;
	}

	public int GetCurrentControllerPosition()
	{
		return currentControllerPosition;
	}

	public void SetCurrentControllerPosition(int i)
	{
		currentControllerPosition = i;
	}

	public int GetCurrentPlayer()
	{
		return currentPlayer;
	}
}
