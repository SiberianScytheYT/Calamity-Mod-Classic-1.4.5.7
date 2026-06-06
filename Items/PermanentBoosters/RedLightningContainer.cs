using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.PermanentBoosters
{
	public class RedLightningContainer : ModItem
	{
		public int frameCounter = 0;
		public int frame = 0;
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Red Lightning Container");
/*
			Tooltip.SetDefault("Permanently makes Rage Mode do 15% more damage\n" +
				"Revengeance drop");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 40;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item122;
			Item.consumable = true;
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.Turquoise;
		}

		public override bool CanUseItem(Player player) => !player.Calamity().rageBoostThree;

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/PermanentBoosters/RedLightningContainer_Animated").Value;
			spriteBatch.Draw(texture, position, Item.GetCurrentFrame(ref frame, ref frameCounter, 5, 6), Color.White, 0f, origin, scale, SpriteEffects.None, 0);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/PermanentBoosters/RedLightningContainer_Animated").Value;
			spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 5, 6), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/PermanentBoosters/RedLightningContainerGlow").Value;
			spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 5, 6, false), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			if (player.itemAnimation > 0 && player.itemTime == 0)
			{
				player.itemTime = Item.useTime;
				CalamityPlayer modPlayer = player.Calamity();
				modPlayer.rageBoostThree = true;
			}
			return true;
		}
	}
}
