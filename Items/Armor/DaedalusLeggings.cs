using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DaedalusLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Daedalus Leggings");
/*
            Tooltip.SetDefault("3% increased critical strike chance\n" +
                "10% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = 5;
            Item.defense = 15; //41
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().AllCritBoost(3);
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 10);
			recipe.AddIngredient(ItemID.CrystalShard, 8);
			recipe.AddIngredient(ModContent.ItemType<EssenceofEleum>(), 2);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
