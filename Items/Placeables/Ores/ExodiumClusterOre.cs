using CalRD.Tiles.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.Ores
{
    public class ExodiumClusterOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Exodium Cluster");
/*
            Tooltip.SetDefault("A cold cluster from the great unknown.");
*/
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<ExodiumOre>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 1, silver: 60);
            Item.rare = 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarOre, 3);
            recipe.AddIngredient(ItemID.FragmentStardust);
            recipe.AddIngredient(ItemID.FragmentSolar);
            recipe.AddIngredient(ItemID.FragmentVortex);
            recipe.AddIngredient(ItemID.FragmentNebula);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
