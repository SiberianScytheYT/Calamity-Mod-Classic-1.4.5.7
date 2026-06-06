using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Items
{
    public class BrokenWaterFilter : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Broken Water Filter");
/*
            Tooltip.SetDefault("Favorite this item to disable natural Acid Rain spawns");
*/
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = 1;
        }
        public override void UpdateInventory(Player player)
        {
			if (Item.favorited)
				player.Calamity().noStupidNaturalARSpawns = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 20);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
