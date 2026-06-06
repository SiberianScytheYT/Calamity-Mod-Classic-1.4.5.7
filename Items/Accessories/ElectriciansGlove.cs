using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ElectriciansGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Electrician's Glove");
/*
            Tooltip.SetDefault("Stealth strikes summon sparks on enemy hits\n"
                               +"Stealth strikes also have +30 armor penetration, deal 10% more damage, and heal for 1 HP");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 40;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.accessory = true;
            Item.rare = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.electricianGlove = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FilthyGlove>());
            recipe.AddIngredient(ItemID.Wire, 100);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodstainedGlove>());
            recipe.AddIngredient(ItemID.Wire, 100);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
