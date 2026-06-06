using CalRD.Items.Placeables.Ores;
using CalRD.Tiles;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class AuricBar : ModItem
    {
		public int frameCounter = 0;
		public int frame = 0;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Auric Tesla Bar");
/*
            Tooltip.SetDefault("It radiates godly energy");
*/
        }

        public override void SetDefaults()
        {
			Item.createTile = ModContent.TileType<AuricTeslaBar>();
            Item.width = 50;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 60);
            Item.Calamity().customRarity = CalamityRarity.Violet;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
        }

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation += new Vector2(-10f * player.direction, 10f * player.gravDir).RotatedBy(player.itemRotation);
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/Materials/AuricBar_Animated").Value;
			spriteBatch.Draw(texture, position, Item.GetCurrentFrame(ref frame, ref frameCounter, 6, 16), Color.White, 0f, origin, scale, SpriteEffects.None, 0);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/Materials/AuricBar_Animated").Value;
			spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 6, 16), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/Materials/AuricBarGlow").Value;
			spriteBatch.Draw(texture, Item.position - Main.screenPosition, Item.GetCurrentFrame(ref frame, ref frameCounter, 6, 16, false), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
		}

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.5f * brightness, 0.7f * brightness, 0f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>());
            recipe.AddIngredient(ModContent.ItemType<AuricOre>(), 20);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 2);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>());
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>());
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>());
            recipe.AddIngredient(ModContent.ItemType<HellcasterFragment>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
