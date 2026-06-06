using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class Starcore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Starcore");
/*
            Tooltip.SetDefault("May the stars guide your way\n" +
                "Summons Astrum Deus at the Astral Beacon but is not consumed.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 40;
            Item.rare = 9;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 25);
            recipe.AddIngredient(ModContent.ItemType<AstralJelly>(), 8);
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
