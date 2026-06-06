using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PlagueReaperStriders : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plague Reaper Striders");
/*
            Tooltip.SetDefault("3% increased critical strike chance\n" +
                "20% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 18, 0, 0);
            Item.rare = 8;
            Item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Ranged) += 3;
            player.moveSpeed += 0.2f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 21);
			recipe.AddIngredient(ItemID.NecroGreaves);
			recipe.AddIngredient(ItemID.Nanites, 17);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
