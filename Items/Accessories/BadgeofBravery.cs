using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class BadgeofBravery : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Badge of Bravery");
/*
            Tooltip.SetDefault("15% increased melee speed\n" +
							   "Wearing Tarragon armor increases melee damage, crit, and armor penetration");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.badgeOfBravery = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 2);
            recipe.AddIngredient(ItemID.FeralClaws);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
