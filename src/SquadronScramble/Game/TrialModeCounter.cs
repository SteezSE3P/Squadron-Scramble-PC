using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace SquadronScramble;

public class TrialModeCounter : GameComponent
{
	private const int TimeOut = 8;

	private bool trialMode = true;

	private bool trialModeTimeout = false;

	private bool showGuide = false;

	private Stopwatch stopWatch;

	private Game game;

	public bool IsTrialMode => trialMode && Guide.IsTrialMode;

	public TrialModeCounter(Game game)
		: base(game)
	{
		this.game = game;
	}

	public override void Initialize()
	{
		stopWatch = new Stopwatch();
		stopWatch.Start();
	}

	public override void Update(GameTime gameTime)
	{
		if (!Guide.IsTrialMode)
		{
			base.Enabled = false;
			return;
		}
		if (stopWatch.Elapsed.Minutes >= 8 && !trialModeTimeout)
		{
			trialModeTimeout = true;
			showGuide = true;
		}
		if (!showGuide || Guide.IsVisible)
		{
			return;
		}
		try
		{
			Guide.BeginShowMessageBox("Time Expired", "The Trial for this community game has\r\nended. You can restart the demo to play\r\nagain, or unlock the game below.\r\n\r\nWould you like to unlock the full game?", new string[2] { "Exit Game", "Unlock Game" }, 0, MessageBoxIcon.Alert, delegate(IAsyncResult result)
			{
				int? num = Guide.EndShowMessageBox(result);
				if (num.HasValue && num.Value == 1)
				{
					trialMode = false;
					base.Enabled = false;
					Guide.SimulateTrialMode = false;
				}
				else
				{
					game.Exit();
				}
			}, null);
			showGuide = false;
		}
		catch
		{
		}
	}
}
