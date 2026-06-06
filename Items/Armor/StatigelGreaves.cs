using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class StatigelGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statigel Greaves");
/*
            Tooltip.SetDefault("5% increased damage and movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = 4;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 6);
            recipe.AddIngredient(ItemID.HellstoneBar, 11);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();
        }
    }
}
