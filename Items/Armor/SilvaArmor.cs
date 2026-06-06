using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class SilvaArmor : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Silva Armor");
/*
            Tooltip.SetDefault("+80 max life\n" +
                       "20% increased movement speed\n" +
                       "12% increased damage and 8% increased critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 72, 0, 0);
            Item.defense = 44;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 80;
            player.moveSpeed += 0.2f;
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.Calamity().AllCritBoost(8);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 10);
            recipe.AddRecipeGroup("AnyGoldBar", 10);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 12);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 3);
            recipe.AddIngredient(ModContent.ItemType<LeadCore>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
