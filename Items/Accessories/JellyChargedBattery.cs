using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class JellyChargedBattery : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jelly-Charged Battery");
/*
            Tooltip.SetDefault("+1 max minions and 7% minion damage\n" +
							   "Minion attacks spawn orbs of energy and inflict Electrified");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.accessory = true;
            Item.rare = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.Calamity().voltaicJelly = true;
			player.Calamity().jellyChargedBattery = true;
            player.maxMinions++;
            player.GetDamage(DamageClass.Summon) += 0.07f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WulfrumBattery>());
            recipe.AddIngredient(ModContent.ItemType<VoltaicJelly>());
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
