using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class XerocCuisses : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Empyrean Cuisses");
/*
            Tooltip.SetDefault("Speed of the cosmos\n" +
					"5% increased rogue damage and critical strike chance\n" +
					"20% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = 10;
            Item.defense = 24;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingCrit += 5;
            player.Calamity().throwingDamage += 0.05f;
            player.moveSpeed += 0.2f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeldiateBar>(), 18);
            recipe.AddIngredient(ItemID.LunarBar, 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
