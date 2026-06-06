using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class CoreofCalamity : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Core of Calamity");
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = 8;
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Materials/CoreofCalamityGlow").Value);
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 3);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 3);
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
