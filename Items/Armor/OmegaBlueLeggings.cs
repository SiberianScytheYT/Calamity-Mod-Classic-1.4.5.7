using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class OmegaBlueLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Omega Blue Tentacles");
/*
            Tooltip.SetDefault("30% increased movement speed\n12% increased damage and 8% increased critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 35, 25, 0);
            Item.rare = 10;
            Item.defense = 22;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.Calamity().AllCritBoost(8);

            player.moveSpeed += 0.3f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 13);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 6);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
