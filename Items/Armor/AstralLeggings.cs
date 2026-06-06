using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class AstralLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Leggings");
/*
            Tooltip.SetDefault("10% increased movement speed\n" +
                               "Treasure and ore detection");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = 9;
            Item.defense = 21;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.findTreasure = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 10);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
