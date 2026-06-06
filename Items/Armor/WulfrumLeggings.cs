using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class WulfrumLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Leggings");
/*
            Tooltip.SetDefault("Movement speed increased by 5%");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 25, 0);
            Item.rare = 1;
            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WulfrumShard>(), 8);
            recipe.AddIngredient(ModContent.ItemType<EnergyCore>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
