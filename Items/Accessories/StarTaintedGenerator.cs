using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class StarTaintedGenerator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star-Tainted Generator");
/*
            Tooltip.SetDefault("+2 max minions and 7% minion damage\n" +
							   "Minion attacks spawn astral explosions and inflict several debuffs");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = 8;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.Calamity().voltaicJelly = true;
			player.Calamity().starbusterCore = true;
			player.Calamity().starTaintedGenerator = true;
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Summon) += 0.07f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<JellyChargedBattery>());
            recipe.AddIngredient(ModContent.ItemType<NuclearRod>());
            recipe.AddIngredient(ModContent.ItemType<StarbusterCore>());
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
