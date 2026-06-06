using CalRD.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Placeables.Furniture.CraftingStations
{
    public class AncientAltar : ModItem
    {
        public override void SetStaticDefaults()
        {
/*
            Tooltip.SetDefault("Used for special crafting");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 3;
            Item.consumable = true;
            Item.value = 0;
            Item.createTile = ModContent.TileType<Tiles.Furniture.CraftingStations.AncientAltar>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<BrimstoneSlag>(), 10);
            recipe.AddIngredient(ModContent.ItemType<CharredOre>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
