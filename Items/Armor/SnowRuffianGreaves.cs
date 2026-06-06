using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
    public class SnowRuffianGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Snow Ruffian Greaves");
/*
            Tooltip.SetDefault("5% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 75, 0);
            Item.rare = 1;
            Item.defense = 1; //4
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("AnySnowBlock", 20);
            recipe.AddRecipeGroup("AnyIceBlock", 10);
            recipe.AddIngredient(ItemID.BorealWood, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
