using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class CoreofEleum : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Core of Eleum");
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
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Materials/CoreofEleumGlow").Value);
		}

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.15f * brightness, 0.05f * brightness, 0.5f * brightness);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ModContent.ItemType<EssenceofEleum>());
            recipe.AddIngredient(ItemID.Ectoplasm);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
