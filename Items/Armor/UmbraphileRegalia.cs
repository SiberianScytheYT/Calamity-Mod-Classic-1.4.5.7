using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class UmbraphileRegalia : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Umbraphile Regalia");
/*
            Tooltip.SetDefault("10% increased rogue damage and 10% increased rogue crit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = 7;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
			player.Calamity().throwingDamage += 0.1f;
			player.Calamity().throwingCrit += 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SolarVeil>(), 18);
			recipe.AddIngredient(ItemID.HallowedBar, 15);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
