using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class SilencingSheath : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Silencing Sheath");
/*
            Tooltip.SetDefault("+20 maximum stealth\n" +
                "Stealth generates 15% faster");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.rogueStealthMax += 0.2f;
            modPlayer.stealthGenStandstill += 0.15f;
            modPlayer.stealthGenMoving += 0.15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyEvilBar", 8);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddRecipeGroup("Boss2Material", 3);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
