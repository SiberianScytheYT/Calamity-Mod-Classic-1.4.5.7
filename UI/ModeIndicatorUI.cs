using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.UI
{
	public class ModeIndicatorUI
	{
		public static readonly Vector2 OffsetToAreaCenter = new Vector2(0f, 4f);
		public static Vector2 DifficultyIconOffset => new Vector2(0f, -16f);
		public static Vector2 ArmageddonIconOffset => new Vector2(11.5f, 7f);
		public static Vector2 DefiledRuneIconOffset => new Vector2(-12f, 7);
		public static void Draw(SpriteBatch spriteBatch)
		{
			// The mode indicator should only be displayed when the inventory is open, to prevent obstruction.
			if (!Main.playerInventory)
				return;

			Texture2D outerAreaTexture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/ModeIndicatorArea", AssetRequestMode.ImmediateLoad).Value;
			Texture2D revengeanceIconTexture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/ModeIndicatorRev", AssetRequestMode.ImmediateLoad).Value;
			Texture2D deathIconTexture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/ModeIndicatorDeath", AssetRequestMode.ImmediateLoad).Value;
			Texture2D armageddonIconTexture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/ModeIndicatorArma", AssetRequestMode.ImmediateLoad).Value;

			// CONSIDER: The defiled rune is scheduled to be removed. Possibly change the 3rd slot to occupy death instead of the
			// death icon completely overtaking revengeance?
			Texture2D defiledRuneIconTexture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/UI/ModeIndicatorRune", AssetRequestMode.ImmediateLoad).Value;

			Vector2 drawCenter = new Vector2(Main.screenWidth - 400f, 72f) + outerAreaTexture.Size() * 0.5f;

			spriteBatch.Draw(outerAreaTexture, drawCenter, null, Color.White, 0f, outerAreaTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

			if (CalamityWorld.revenge)
			{
				Texture2D difficultyIconToUse = CalamityWorld.death ? deathIconTexture : revengeanceIconTexture;
				spriteBatch.Draw(difficultyIconToUse, drawCenter + DifficultyIconOffset, null, Color.White, 0f, revengeanceIconTexture.Size() * 0.5f, 0.9f, SpriteEffects.None, 0f);
			}

			if (CalamityWorld.armageddon)
				spriteBatch.Draw(armageddonIconTexture, drawCenter + ArmageddonIconOffset, null, Color.White, 0f, revengeanceIconTexture.Size() * 0.5f, 0.9f, SpriteEffects.None, 0f);

			if (CalamityWorld.defiled)
				spriteBatch.Draw(defiledRuneIconTexture, drawCenter + DefiledRuneIconOffset, null, Color.White, 0f, revengeanceIconTexture.Size() * 0.5f, 0.9f, SpriteEffects.None, 0f);
		}
	}
}
