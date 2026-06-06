using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SilvaLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Silva Leggings");
/*
            Tooltip.SetDefault("45% increased movement speed\n" +
                "12% increased damage and 7% increased critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 54, 0, 0);
            Item.defense = 39;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.45f;
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.Calamity().AllCritBoost(7);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 7);
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 7);
            recipe.AddRecipeGroup("AnyGoldBar", 7);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 9);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
