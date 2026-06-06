using CalRD.Items.Materials;
using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class FilthyGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Filthy Glove");
/*
            Tooltip.SetDefault("Stealth strikes have +10 armor penetration and deal 10% more damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.accessory = true;
            Item.rare = 3;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.filthyGlove = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RottenChunk, 4);
            recipe.AddIngredient(ItemID.DemoniteBar, 4);
            recipe.AddIngredient(ModContent.ItemType<TrueShadowScale>(), 5);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
