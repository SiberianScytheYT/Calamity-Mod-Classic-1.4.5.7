using CalRD.Items.Placeables.Ores;
using CalRD.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Materials
{
    public class UeliaceBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Uelibloom Bar");
        }

        public override void SetDefaults()
        {
			Item.createTile = ModContent.TileType<UelibloomBar>();
            Item.width = 15;
            Item.height = 12;
            Item.maxStack = 999;
            Item.rare = 10;
            Item.value = Item.sellPrice(gold: 3);
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
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
            recipe.AddIngredient(ModContent.ItemType<UelibloomOre>(), 5);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.Register();
        }
    }
}
