using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class VampiricTalisman : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vampiric Talisman");
/*
            Tooltip.SetDefault("Rogue projectiles give lifesteal on crits\n12% increased rogue damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity6BuyPrice;
            Item.accessory = true;
            Item.rare = 6;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.vampiricTalisman = true;
            player.Calamity().throwingDamage += 0.12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RogueEmblem>());
            recipe.AddIngredient(ModContent.ItemType<SolarVeil>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
