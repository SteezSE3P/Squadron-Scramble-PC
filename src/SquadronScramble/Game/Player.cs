using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace SquadronScramble;

public class Player : Participant
{
	private GameWorld g;

	private PlayerIndex currentPlayerIndex;

	private int currentPlayerIndexNumber;

	private bool fireDepressed = false;

	private bool startDepressed = false;

	private bool exploDepressed = false;

	private float ANALOGUEPLANETURN = 0.1f;

	private float ANALOGUEPILOTTURN = 0.2f;

	private float ANALOGUETRIGGERON = 0.8f;

	private float ANALOGUETRIGGEROFF = 0.2f;

	public Player(GameWorld gw)
	{
		g = gw;
	}

	public void Update(GameTime theGameTime, Pilot p)
	{
		Plane thePlane = p.GetThePlane();
		if (!g.theControllerSelectScreen.GetControllerFull(currentPlayerIndexNumber))
		{
			SetCurrentControllerPosition(0);
		}
		if (!GetParticipating())
		{
			return;
		}
		UpdateUniversalControls(theGameTime);
		if ((GetCurrentControllerPosition() == 0 || GetCurrentControllerPosition() == 1) && GamePad.GetState(currentPlayerIndex).Buttons.LeftStick == ButtonState.Pressed && thePlane.GetThePilot().GetCurrentSquadronMember().GetAlive())
		{
			p.FlashControllerIcon(theGameTime);
		}
		if ((GetCurrentControllerPosition() == 0 || GetCurrentControllerPosition() == 2) && GamePad.GetState(currentPlayerIndex).Buttons.RightStick == ButtonState.Pressed && thePlane.GetThePilot().GetCurrentSquadronMember().GetAlive())
		{
			p.FlashControllerIcon(theGameTime);
		}
		if (!thePlane.GetInControl())
		{
			if (GetCurrentControllerPosition() == 0)
			{
				if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X < 0f - ANALOGUEPILOTTURN)
				{
					p.MoveLeft(theGameTime);
				}
				else if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X > ANALOGUEPILOTTURN)
				{
					p.MoveRight(theGameTime);
				}
				else if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X < 0f - ANALOGUEPILOTTURN)
				{
					p.MoveLeft(theGameTime);
				}
				else if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X > ANALOGUEPILOTTURN)
				{
					p.MoveRight(theGameTime);
				}
				else if (GamePad.GetState(currentPlayerIndex).DPad.Left == ButtonState.Pressed)
				{
					p.MoveLeft(theGameTime);
				}
				else if (GamePad.GetState(currentPlayerIndex).DPad.Right == ButtonState.Pressed)
				{
					p.MoveRight(theGameTime);
				}
				else
				{
					p.NoLean(theGameTime);
				}
				if (GamePad.GetState(currentPlayerIndex).Buttons.A == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.B == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.X == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.Y == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Triggers.Left > ANALOGUETRIGGERON || GamePad.GetState(currentPlayerIndex).Triggers.Right > ANALOGUETRIGGERON || GamePad.GetState(currentPlayerIndex).Buttons.LeftShoulder == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.RightShoulder == ButtonState.Pressed)
				{
					if (!fireDepressed)
					{
						p.FirePressed(theGameTime);
						fireDepressed = true;
					}
				}
				else
				{
					fireDepressed = false;
				}
			}
			else if (GetCurrentControllerPosition() == 1)
			{
				if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X < 0f - ANALOGUEPILOTTURN)
				{
					p.MoveLeft(theGameTime);
				}
				if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X > ANALOGUEPILOTTURN)
				{
					p.MoveRight(theGameTime);
				}
				if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X >= 0f - ANALOGUEPILOTTURN && GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X <= ANALOGUEPILOTTURN)
				{
					if (GamePad.GetState(currentPlayerIndex).DPad.Left == ButtonState.Pressed)
					{
						p.MoveLeft(theGameTime);
					}
					else if (GamePad.GetState(currentPlayerIndex).DPad.Right == ButtonState.Pressed)
					{
						p.MoveRight(theGameTime);
					}
					else
					{
						p.NoLean(theGameTime);
					}
				}
				if (GamePad.GetState(currentPlayerIndex).Triggers.Left > ANALOGUETRIGGERON || GamePad.GetState(currentPlayerIndex).Buttons.LeftShoulder == ButtonState.Pressed)
				{
					if (!fireDepressed)
					{
						p.FirePressed(theGameTime);
						fireDepressed = true;
					}
				}
				else
				{
					fireDepressed = false;
				}
			}
			else if (GetCurrentControllerPosition() == 2)
			{
				if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X < 0f - ANALOGUEPILOTTURN)
				{
					p.MoveLeft(theGameTime);
				}
				if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X > ANALOGUEPILOTTURN)
				{
					p.MoveRight(theGameTime);
				}
				if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X >= 0f - ANALOGUEPILOTTURN && GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X <= ANALOGUEPILOTTURN)
				{
					if (GamePad.GetState(currentPlayerIndex).Buttons.A == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.X == ButtonState.Pressed)
					{
						p.MoveLeft(theGameTime);
					}
					else if (GamePad.GetState(currentPlayerIndex).Buttons.B == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.Y == ButtonState.Pressed)
					{
						p.MoveRight(theGameTime);
					}
					else
					{
						p.NoLean(theGameTime);
					}
				}
				if (GamePad.GetState(currentPlayerIndex).Triggers.Right > ANALOGUETRIGGERON || GamePad.GetState(currentPlayerIndex).Buttons.RightShoulder == ButtonState.Pressed)
				{
					if (!fireDepressed)
					{
						p.FirePressed(theGameTime);
						fireDepressed = true;
					}
				}
				else
				{
					fireDepressed = false;
				}
			}
		}
		if (!thePlane.GetInControl())
		{
			return;
		}
		if (GetCurrentControllerPosition() == 0)
		{
			if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X < 0f - ANALOGUEPLANETURN)
			{
				thePlane.AnalogueLeft(GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X, theGameTime);
			}
			else if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X > ANALOGUEPLANETURN)
			{
				thePlane.AnalogueRight(GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X, theGameTime);
			}
			else if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X < 0f - ANALOGUEPLANETURN)
			{
				thePlane.AnalogueLeft(GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X, theGameTime);
			}
			else if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X > ANALOGUEPLANETURN)
			{
				thePlane.AnalogueRight(GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X, theGameTime);
			}
			else if (GamePad.GetState(currentPlayerIndex).DPad.Left == ButtonState.Pressed)
			{
				thePlane.TurnLeft(theGameTime);
			}
			else if (GamePad.GetState(currentPlayerIndex).DPad.Right == ButtonState.Pressed)
			{
				thePlane.TurnRight(theGameTime);
			}
			else
			{
				thePlane.NoTurn(theGameTime);
			}
			if (GamePad.GetState(currentPlayerIndex).Buttons.A == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.B == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.X == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.Y == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Triggers.Left > ANALOGUETRIGGERON || GamePad.GetState(currentPlayerIndex).Triggers.Right > ANALOGUETRIGGERON || GamePad.GetState(currentPlayerIndex).Buttons.LeftShoulder == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.RightShoulder == ButtonState.Pressed)
			{
				if (!fireDepressed)
				{
					thePlane.Shoot(theGameTime);
					fireDepressed = true;
				}
			}
			else
			{
				fireDepressed = false;
			}
		}
		else if (GetCurrentControllerPosition() == 1)
		{
			if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X < 0f - ANALOGUEPLANETURN)
			{
				thePlane.AnalogueLeft(GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X, theGameTime);
			}
			if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X > ANALOGUEPLANETURN)
			{
				thePlane.AnalogueRight(GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X, theGameTime);
			}
			if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X >= 0f - ANALOGUEPLANETURN && GamePad.GetState(currentPlayerIndex).ThumbSticks.Left.X <= ANALOGUEPLANETURN)
			{
				if (GamePad.GetState(currentPlayerIndex).DPad.Left == ButtonState.Pressed)
				{
					thePlane.TurnLeft(theGameTime);
				}
				else if (GamePad.GetState(currentPlayerIndex).DPad.Right == ButtonState.Pressed)
				{
					thePlane.TurnRight(theGameTime);
				}
				else
				{
					thePlane.NoTurn(theGameTime);
				}
			}
			if (GamePad.GetState(currentPlayerIndex).Triggers.Left > ANALOGUETRIGGERON || GamePad.GetState(currentPlayerIndex).Buttons.LeftShoulder == ButtonState.Pressed)
			{
				if (!fireDepressed)
				{
					thePlane.Shoot(theGameTime);
					fireDepressed = true;
				}
			}
			else
			{
				fireDepressed = false;
			}
		}
		else
		{
			if (GetCurrentControllerPosition() != 2)
			{
				return;
			}
			if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X < 0f - ANALOGUEPLANETURN)
			{
				thePlane.AnalogueLeft(GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X, theGameTime);
			}
			if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X > ANALOGUEPLANETURN)
			{
				thePlane.AnalogueRight(GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X, theGameTime);
			}
			if (GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X >= 0f - ANALOGUEPLANETURN && GamePad.GetState(currentPlayerIndex).ThumbSticks.Right.X <= ANALOGUEPLANETURN)
			{
				if (GamePad.GetState(currentPlayerIndex).Buttons.A == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.X == ButtonState.Pressed)
				{
					thePlane.TurnLeft(theGameTime);
				}
				else if (GamePad.GetState(currentPlayerIndex).Buttons.B == ButtonState.Pressed || GamePad.GetState(currentPlayerIndex).Buttons.Y == ButtonState.Pressed)
				{
					thePlane.TurnRight(theGameTime);
				}
				else
				{
					thePlane.NoTurn(theGameTime);
				}
			}
			if (GamePad.GetState(currentPlayerIndex).Triggers.Right > ANALOGUETRIGGERON || GamePad.GetState(currentPlayerIndex).Buttons.RightShoulder == ButtonState.Pressed)
			{
				if (!fireDepressed)
				{
					thePlane.Shoot(theGameTime);
					fireDepressed = true;
				}
			}
			else
			{
				fireDepressed = false;
			}
		}
	}

	public void UpdateUniversalControls(GameTime theGameTime)
	{
		if (Guide.IsVisible)
		{
			g.thePauseManager.GuideVisible(theGameTime);
		}
		if (g.theControllerMenuManager[currentPlayerIndexNumber].CheckButtonStartPressed())
		{
			g.thePauseManager.PausePressed(theGameTime, this);
		}
		if (CustomOptions.GetDebugOn())
		{
			if (GamePad.GetState(currentPlayerIndex).Buttons.RightShoulder == ButtonState.Pressed)
			{
				CustomOptions.SetGameSpeed(CustomOptions.GetGameSpeed() + 0.05f * (float)theGameTime.ElapsedGameTime.TotalSeconds);
			}
			if (GamePad.GetState(currentPlayerIndex).Buttons.LeftShoulder == ButtonState.Pressed)
			{
				CustomOptions.SetGameSpeed(CustomOptions.GetGameSpeed() - 0.05f * (float)theGameTime.ElapsedGameTime.TotalSeconds);
			}
			if (CustomOptions.GetGameSpeed() >= 1f)
			{
				CustomOptions.SetGameSpeed(1f);
			}
		}
	}

	public void SetCurrentPlayerIndex(int pi)
	{
		SetCurrentPlayer(pi);
		currentPlayerIndexNumber = pi;
		if (currentPlayerIndexNumber == 0)
		{
			currentPlayerIndex = PlayerIndex.One;
		}
		else if (currentPlayerIndexNumber == 1)
		{
			currentPlayerIndex = PlayerIndex.Two;
		}
		else if (currentPlayerIndexNumber == 2)
		{
			currentPlayerIndex = PlayerIndex.Three;
		}
		else if (currentPlayerIndexNumber == 3)
		{
			currentPlayerIndex = PlayerIndex.Four;
		}
	}

	public int GetCurrentPlayerIndexNumber()
	{
		return currentPlayerIndexNumber;
	}

	public void SetStartDepressed(bool b)
	{
		startDepressed = b;
	}

	public PlayerIndex GetCurrentPlayerIndex()
	{
		return currentPlayerIndex;
	}

	public bool GetStartDepressed()
	{
		return startDepressed;
	}
}
