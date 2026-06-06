using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class UmbraphileBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Umbraphile Boots");
/*
            Tooltip.SetDefault("9% increased rogue damage and 6% increased rogue crit\n" +
                               "30% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 18, 0, 0);
            Item.rare = 7;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.3f;
			player.Calamity().throwingDamage += 0.09f;
			player.Calamity().throwingCrit += 6;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SolarVeil>(), 14);
			recipe.AddIngredient(ItemID.HallowedBar, 11);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
