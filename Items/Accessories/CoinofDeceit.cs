using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class CoinofDeceit : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coin of Deceit");
/*
            Tooltip.SetDefault("Stealth strikes only expend 75% of your max stealth\n" +
			"6% increased rogue crit chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.accessory = true;
            Item.rare = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Calamity().throwingCrit += 6;
            player.Calamity().stealthStrike75Cost = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnyGoldBar", 4);
            recipe.AddRecipeGroup("AnyCopperBar", 8);
			// So you make fake coins out of wood, nobody will judge you
            recipe.AddIngredient(ModContent.ItemType<Acidwood>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
