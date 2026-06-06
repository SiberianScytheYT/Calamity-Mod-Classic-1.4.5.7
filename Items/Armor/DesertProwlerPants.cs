using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DesertProwlerPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desert Prowler Pants");
/*
            Tooltip.SetDefault("10% increased movement speed and immunity to the Mighty Wind debuff");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = 1;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.WindPushed] = true;
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DesertFeather>());
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
    }
}
