using CalRD.Items.Materials;
using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class BloodstainedGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodstained Glove");
/*
            Tooltip.SetDefault("Stealth strikes have +10 armor penetration and heal for 1 HP");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.accessory = true;
            Item.rare = 3;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.bloodyGlove = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodSample>(), 5);
            recipe.AddIngredient(ItemID.Vertebrae, 4);
            recipe.AddIngredient(ItemID.CrimtaneBar, 4);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
