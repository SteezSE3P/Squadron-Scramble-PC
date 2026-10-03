using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquadronScramble;

public static class RoundResultScenery
{
	private static GameWorld g;

	private static Texture2D elementsTexture;

	private static int NUMBEROFCLOUDS = 3;

	private static Vector2[] cloudPosition;

	private static float[] cloudSpeed;

	public static void SetRoundResultScenery(GameWorld gw)
	{
		g = gw;
		cloudPosition = new Vector2[NUMBEROFCLOUDS];
		cloudSpeed = new float[NUMBEROFCLOUDS];
		ref Vector2 reference = ref cloudPosition[0];
		reference = new Vector2(200f, 150f);
		ref Vector2 reference2 = ref cloudPosition[1];
		reference2 = new Vector2(800f, 500f);
		ref Vector2 reference3 = ref cloudPosition[2];
		reference3 = new Vector2(-1500f, 850f);
		cloudSpeed[0] = 10f;
		cloudSpeed[1] = 20f;
		cloudSpeed[2] = 25f;
	}

	public static void LoadContent(ContentManager theContentManager)
	{
		elementsTexture = TextureManager.GetElementsFrontEndTexture();
	}

	public static void Draw(SpriteBatch theSpriteBatch)
	{
		DrawClouds(theSpriteBatch);
	}

	public static void DrawClouds(SpriteBatch theSpriteBatch)
	{
		Vector2 origin = new Vector2(200f, 170f);
		Rectangle value = new Rectangle(447, 256, 417, 177);
		for (int i = 0; i < NUMBEROFCLOUDS; i++)
		{
			theSpriteBatch.Draw(elementsTexture, cloudPosition[i], value, Color.White, 0f, origin, 3f, SpriteEffects.None, 0f);
		}
	}

	public static void Update(GameTime theGameTime)
	{
		for (int i = 0; i < NUMBEROFCLOUDS; i++)
		{
			cloudPosition[i].X -= cloudSpeed[i] * (float)theGameTime.ElapsedGameTime.TotalSeconds;
			if (cloudPosition[i].X < -1100f)
			{
				cloudPosition[i].X = 2300f;
			}
		}
	}
}
