using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class AtaxiaSubligar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hydrothermic Subligar");
/*
            Tooltip.SetDefault("3% increased critical strike chance\n" +
                "15% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 18, 0, 0);
            Item.rare = 8;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 3;
            player.GetCritChance(DamageClass.Magic) += 3;
            player.GetCritChance(DamageClass.Ranged) += 3;
            player.Calamity().throwingCrit += 3;
            player.moveSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 10);
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 2);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
