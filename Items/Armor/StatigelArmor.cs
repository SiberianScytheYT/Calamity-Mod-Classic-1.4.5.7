using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class StatigelArmor : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Statigel Armor");
/*
            Tooltip.SetDefault("5% increased critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 4;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
			player.Calamity().AllCritBoost(5);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 8);
            recipe.AddIngredient(ItemID.HellstoneBar, 13);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();
        }
    }
}
