using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class NecklaceofVexation : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Necklace of Vexation");
/*
            Tooltip.SetDefault("Revenge\n" +
            "20% increased damage when under 50% life\n" +
			"All attacks inflict Cursed Inferno and Venom while wearing Reaver armor");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 34;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.vexation = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 2);
            recipe.AddIngredient(ItemID.AvengerEmblem);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
