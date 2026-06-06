using CalRD.Items.Placeables.Ores;
using CalRD.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
	public class CruptixBar : ModItem
	{
		public int frameCounter = 0;
		public int frame = 0;
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Scoria Bar");
/*
			Tooltip.SetDefault("The smoke feels warm");
*/
		}

		public override void SetDefaults()
		{
			Item.createTile = ModContent.TileType<ChaoticBarPlaced>();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.width = 40;
			Item.height = 52;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(gold: 1, silver: 20);
			Item.rare = 8;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/Materials/CruptixBar_Animated").Value;
			spriteBatch.Draw(texture, position, Item.GetCurrentFrame(ref frame, ref frameCounter, 6, 6), Color.White, 0f, origin, scale, SpriteEffects.None, 0);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/Materials/CruptixBar_Animated").Value;
			spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 6, 6), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ChaoticOre>(), 5);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.Register();
		}
	}
}
