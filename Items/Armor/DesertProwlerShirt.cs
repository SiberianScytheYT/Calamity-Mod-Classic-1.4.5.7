using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
    public class DesertProwlerShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desert Prowler Shirt");
/*
            Tooltip.SetDefault("5% increased ranged critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = 1;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Ranged) += 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DesertFeather>(), 3);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
    }
}
