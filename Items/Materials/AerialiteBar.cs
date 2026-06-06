using CalRD.Items.Placeables.Ores;
using CalRD.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class AerialiteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aerialite Bar");
        }

        public override void SetDefaults()
        {
			Item.createTile = ModContent.TileType<AerialiteBarTile>();
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 30);
            Item.rare = 3;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteOre>(), 4);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}
