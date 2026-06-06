using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ReaverCuisses : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaver Cuisses");
/*
            Tooltip.SetDefault("5% increased critical strike chance\n" +
                "12% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 18, 0, 0);
            Item.rare = 7;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Melee) += 5;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.Calamity().throwingCrit += 5;
            player.moveSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 10);
			recipe.AddIngredient(ItemID.JungleSpores, 8);
			recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 2);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
