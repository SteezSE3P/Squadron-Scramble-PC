using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SquadronScramble;

public class ControllerMenuManager
{
	private GameWorld g;

	private PlayerIndex thePlayerIndex;

	private bool leftUpPressed = false;

	private bool leftUpRegistered = false;

	private bool leftDownPressed = false;

	private bool leftDownRegistered = false;

	private bool leftLeftPressed = false;

	private bool leftLeftRegistered = false;

	private bool leftRightPressed = false;

	private bool leftRightRegistered = false;

	private bool rightUpPressed = false;

	private bool rightUpRegistered = false;

	private bool rightDownPressed = false;

	private bool rightDownRegistered = false;

	private bool rightLeftPressed = false;

	private bool rightLeftRegistered = false;

	private bool rightRightPressed = false;

	private bool rightRightRegistered = false;

	private bool buttonAPressed = false;

	private bool buttonARegistered = false;

	private bool buttonBPressed = false;

	private bool buttonBRegistered = false;

	private bool buttonYPressed = false;

	private bool buttonYRegistered = false;

	private bool buttonStartPressed = false;

	private bool buttonStartRegistered = false;

	private bool buttonBackPressed = false;

	private bool buttonBackRegistered = false;

	private float ANALOGUETHRESHOLD = 0.5f;

	public ControllerMenuManager(GameWorld gw, PlayerIndex pi)
	{
		g = gw;
		thePlayerIndex = pi;
	}

	public void Update(GameTime theGameTime)
	{
		CheckControls(theGameTime);
	}

	public void CheckControls(GameTime theGameTime)
	{
		if (GamePad.GetState(thePlayerIndex).DPad.Up == ButtonState.Pressed || GamePad.GetState(thePlayerIndex).ThumbSticks.Left.Y > ANALOGUETHRESHOLD)
		{
			leftUpPressed = true;
		}
		else
		{
			leftUpPressed = false;
			leftUpRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).DPad.Down == ButtonState.Pressed || GamePad.GetState(thePlayerIndex).ThumbSticks.Left.Y < 0f - ANALOGUETHRESHOLD)
		{
			leftDownPressed = true;
		}
		else
		{
			leftDownPressed = false;
			leftDownRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).DPad.Left == ButtonState.Pressed || GamePad.GetState(thePlayerIndex).ThumbSticks.Left.X < 0f - ANALOGUETHRESHOLD)
		{
			leftLeftPressed = true;
		}
		else
		{
			leftLeftPressed = false;
			leftLeftRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).DPad.Right == ButtonState.Pressed || GamePad.GetState(thePlayerIndex).ThumbSticks.Left.X > ANALOGUETHRESHOLD)
		{
			leftRightPressed = true;
		}
		else
		{
			leftRightPressed = false;
			leftRightRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).ThumbSticks.Right.Y > ANALOGUETHRESHOLD)
		{
			rightUpPressed = true;
		}
		else
		{
			rightUpPressed = false;
			rightUpRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).ThumbSticks.Right.Y < 0f - ANALOGUETHRESHOLD)
		{
			rightDownPressed = true;
		}
		else
		{
			rightDownPressed = false;
			rightDownRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).ThumbSticks.Right.X < 0f - ANALOGUETHRESHOLD)
		{
			rightLeftPressed = true;
		}
		else
		{
			rightLeftPressed = false;
			rightLeftRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).ThumbSticks.Right.X > ANALOGUETHRESHOLD)
		{
			rightRightPressed = true;
		}
		else
		{
			rightRightPressed = false;
			rightRightRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).Buttons.A == ButtonState.Pressed)
		{
			buttonAPressed = true;
		}
		else
		{
			buttonAPressed = false;
			buttonARegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).Buttons.B == ButtonState.Pressed)
		{
			buttonBPressed = true;
		}
		else
		{
			buttonBPressed = false;
			buttonBRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).Buttons.Y == ButtonState.Pressed)
		{
			buttonYPressed = true;
		}
		else
		{
			buttonYPressed = false;
			buttonYRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).Buttons.Start == ButtonState.Pressed)
		{
			buttonStartPressed = true;
		}
		else
		{
			buttonStartPressed = false;
			buttonStartRegistered = false;
		}
		if (GamePad.GetState(thePlayerIndex).Buttons.Back == ButtonState.Pressed)
		{
			buttonBackPressed = true;
			return;
		}
		buttonBackPressed = false;
		buttonBackRegistered = false;
	}

	public bool GetIsConnected()
	{
		return GamePad.GetState(thePlayerIndex).IsConnected;
	}

	public bool CheckLeftUpPressed()
	{
		if (leftUpPressed && !leftUpRegistered)
		{
			leftUpRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckLeftDownPressed()
	{
		if (leftDownPressed && !leftDownRegistered)
		{
			leftDownRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckLeftLeftPressed()
	{
		if (leftLeftPressed && !leftLeftRegistered)
		{
			leftLeftRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckLeftRightPressed()
	{
		if (leftRightPressed && !leftRightRegistered)
		{
			leftRightRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckRightUpPressed()
	{
		if (rightUpPressed && !rightUpRegistered)
		{
			rightUpRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckRightDownPressed()
	{
		if (rightDownPressed && !rightDownRegistered)
		{
			rightDownRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckRightLeftPressed()
	{
		if (rightLeftPressed && !rightLeftRegistered)
		{
			rightLeftRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckRightRightPressed()
	{
		if (rightRightPressed && !rightRightRegistered)
		{
			rightRightRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckButtonAPressed()
	{
		if (buttonAPressed && !buttonARegistered)
		{
			buttonARegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckButtonBPressed()
	{
		if (buttonBPressed && !buttonBRegistered)
		{
			buttonBRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckButtonYPressed()
	{
		if (buttonYPressed && !buttonYRegistered)
		{
			buttonYRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckButtonStartPressed()
	{
		if (buttonStartPressed && !buttonStartRegistered)
		{
			buttonStartRegistered = true;
			return true;
		}
		return false;
	}

	public bool CheckButtonBackPressed()
	{
		if (buttonBackPressed && !buttonBackRegistered)
		{
			buttonBackRegistered = true;
			return true;
		}
		return false;
	}

	public PlayerIndex GetPlayerIndex()
	{
		return thePlayerIndex;
	}
}
