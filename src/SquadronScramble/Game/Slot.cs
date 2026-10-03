using Microsoft.Xna.Framework;

namespace SquadronScramble;

public class Slot
{
	private Squadron currentSquadron;

	private bool participating = true;

	private string slotName;

	private string slotFullName;

	private Color slotColorA;

	private Color slotColorB;

	private Color slotColorC;

	private Color slotColorD;

	public Slot(Squadron s, Color a, Color b, Color c, Color d, string st, string fn)
	{
		SetCurrentSquadron(s);
		slotColorA = a;
		slotColorB = b;
		slotColorC = c;
		slotColorD = d;
		slotName = st;
		slotFullName = fn;
		currentSquadron.SetTheColor(slotColorA);
	}

	public void FullReset()
	{
		participating = true;
	}

	public void SetCurrentSquadron(Squadron s)
	{
		currentSquadron = s;
		currentSquadron.SetTheColor(slotColorA);
	}

	public void SetParticipating(bool b)
	{
		participating = b;
	}

	public Squadron GetCurrentSquadron()
	{
		return currentSquadron;
	}

	public bool GetParticipating()
	{
		return participating;
	}

	public Color GetSlotColorA()
	{
		return slotColorA;
	}

	public Color GetSlotColorB()
	{
		return slotColorB;
	}

	public Color GetSlotColorC()
	{
		return slotColorC;
	}

	public Color GetSlotColorD()
	{
		return slotColorD;
	}

	public string GetSlotName()
	{
		return slotName;
	}

	public string GetSlotFullName()
	{
		return slotFullName;
	}
}
