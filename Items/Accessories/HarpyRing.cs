using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class HarpyRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Harpy Ring");
/*
            Tooltip.SetDefault("20% increased movement speed\n" +
                "Boosts your maximum flight time by 25%");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.harpyRing = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 2);
            recipe.AddIngredient(ItemID.Feather, 5);
            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }
    }
}
