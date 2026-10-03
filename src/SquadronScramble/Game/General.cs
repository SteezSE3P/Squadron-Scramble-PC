using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public static class General
{
	private static Random random = new Random();

	private static float optionThrobValue = 0f;

	private static float optionThrobTimer = 0f;

	private static string theString = "";

	public static void Update(GameTime theGameTime)
	{
		UpdateOptionThrob(theGameTime);
	}

	public static void UpdateOptionThrob(GameTime theGameTime)
	{
		optionThrobTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
		optionThrobValue = (float)Math.Sin(optionThrobTimer * 3f);
		if (optionThrobValue <= 0f)
		{
			optionThrobTimer = 0f;
		}
	}

	public static float GetOptionThrobValue()
	{
		return 1f - Math.Abs(optionThrobValue * 0.2f);
	}

	public static void DrawOutlineString(SpriteBatch theSpriteBatch, SpriteFont f, string s, Vector2 v, Color c, float r, Vector2 o, float sc, int b, float fade)
	{
		theString = s;
		theSpriteBatch.DrawString(f, theString, v + new Vector2(-b, -b), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(b, -b), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(-b, b), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(b, b), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v, c * fade, r, o, sc, SpriteEffects.None, 0f);
	}

	public static void DrawEmbossedString(SpriteBatch theSpriteBatch, SpriteFont f, string s, Vector2 v, Color c, Color ct, Color cb, float r, Vector2 o, float sc, int b, float fade)
	{
		theString = s;
		theSpriteBatch.DrawString(f, theString, v + new Vector2(-b - 1, -b - 1), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(b + 1, -b - 1), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(-b - 1, b + 1), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(b + 1, b + 1), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(-b - 1, 0f), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(b + 1, 0f), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(0f, -b - 1), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(0f, b + 1), Color.Black * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(b, b), cb * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(-b, -b), ct * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(-b, b), cb * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(b, -b), ct * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(-b, 0f), cb * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(b, 0f), ct * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(0f, -b), ct * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v + new Vector2(0f, b), cb * fade, r, o, sc, SpriteEffects.None, 0f);
		theSpriteBatch.DrawString(f, theString, v, c * fade, r, o, sc, SpriteEffects.None, 0f);
	}

	public static bool isBetween(float f, float minX, float maxX)
	{
		if (f >= minX && f <= maxX)
		{
			return true;
		}
		return false;
	}

	public static float CheckDistance(Vector2 a, Vector2 b)
	{
		float num = a.X - b.X;
		float num2 = a.Y - b.Y;
		return (float)Math.Sqrt(num * num + num2 * num2);
	}

	public static float CheckDistanceX(float a, float b)
	{
		float num = a - b;
		float num2 = 0f;
		return (float)Math.Sqrt(num * num + num2 * num2);
	}

	public static double GetDirectionToTarget(Vector2 v, Vector2 t)
	{
		Vector2 vector = v;
		Vector2 vector2 = t;
		float num = t.X - v.X;
		float num2 = t.Y - v.Y;
		if (num == 0f)
		{
			if (v.Y < t.Y)
			{
				return 1.5707963705062866;
			}
			return 4.71238911151886;
		}
		double num3 = Math.Atan(Math.Abs(num2) / Math.Abs(num));
		if (num2 >= 0f && num < 0f)
		{
			num3 = 1.5707963705062866 - num3 + 1.5707963705062866;
		}
		if (num2 < 0f && num < 0f)
		{
			num3 += 3.1415927410125732;
		}
		if (num2 < 0f && num >= 0f)
		{
			num3 = 1.5707963705062866 - num3 + 4.71238911151886;
		}
		return num3;
	}

	public static bool IsInDirectionRange(Vector2 subjectPosition, Vector2 targetPosition, double subjectDirection, double range)
	{
		if (RelativeAngle(subjectPosition, targetPosition, subjectDirection) > 6.2831854820251465 - range / 2.0 || RelativeAngle(subjectPosition, targetPosition, subjectDirection) < range / 2.0)
		{
			return true;
		}
		return false;
	}

	public static bool IsDirectionSideClockwise(Vector2 subjectPosition, Vector2 targetPosition, double subjectDirection, double range)
	{
		if (RelativeAngle(subjectPosition, targetPosition, subjectDirection) < range)
		{
			return true;
		}
		return false;
	}

	public static double RelativeAngle(Vector2 subjectPosition, Vector2 targetPosition, double subjectDirection)
	{
		return Math.Abs((GetDirectionToTarget(subjectPosition, targetPosition) - subjectDirection % 6.2831854820251465 + 6.2831854820251465) % 6.2831854820251465);
	}

	public static double RelativeAgnosticAngle(Vector2 subjectPosition, Vector2 targetPosition, double subjectDirection)
	{
		double num = Math.Abs((GetDirectionToTarget(subjectPosition, targetPosition) - subjectDirection % 6.2831854820251465 + 6.2831854820251465) % 6.2831854820251465);
		if (num > 3.1415927410125732)
		{
			num = 6.2831854820251465 - num;
		}
		return num;
	}

	public static double RelativeAgnosticNormalisedAngle(Vector2 subjectPosition, Vector2 targetPosition, double subjectDirection, double range)
	{
		double num = Math.Abs((GetDirectionToTarget(subjectPosition, targetPosition) - subjectDirection % 6.2831854820251465 + 6.2831854820251465) % 6.2831854820251465);
		if (num > 3.1415927410125732)
		{
			num = 6.2831854820251465 - num;
		}
		if (num <= range)
		{
			return num / range;
		}
		return 1.0;
	}

	// PC port: number of random draws so far. Online play compares it between PCs to detect desyncs.
	public static long RandomCallCount;

	public static int GetNextRandom(int i, int m)
	{
		RandomCallCount++;
		return random.Next(i, m);
	}

	/// <summary>PC port: online play seeds every PC with the same value so the simulations match.</summary>
	public static void SeedRandom(int seed)
	{
		random = new Random(seed);
		RandomCallCount = 0;
	}
}
