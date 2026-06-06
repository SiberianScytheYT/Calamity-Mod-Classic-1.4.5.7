using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class LumenousAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lumenous Amulet");
/*
            Tooltip.SetDefault("Attacks inflict the Crush Depth debuff\n" +
                "While in the abyss you gain 25% increased max life\n" +
                "Provides a moderate amount of light in the abyss");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.abyssalAmulet = true;
            modPlayer.lumenousAmulet = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AbyssalAmulet>());
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
