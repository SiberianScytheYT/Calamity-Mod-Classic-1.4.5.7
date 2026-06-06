using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class CoreofCinder : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Core of Sunlight");
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 40);
            Item.rare = 8;
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Materials/CoreofCinderGlow").Value);
		}

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.3f * brightness, 0.3f * brightness, 0.05f * brightness);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>());
            recipe.AddIngredient(ItemID.Ectoplasm);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
