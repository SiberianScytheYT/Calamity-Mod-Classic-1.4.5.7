using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class OmegaBlueChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Omega Blue Chestplate");
/*
            Tooltip.SetDefault("12% increased damage and 8% increased critical strike chance\n"
                               +"Your attacks inflict Crush Depth\n"
                               +"No positive life regen");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 38, 0, 0);
            Item.rare = 10;
            Item.defense = 28;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.GetDamage(DamageClass.Generic) += 0.12f;
            modPlayer.AllCritBoost(8);
            modPlayer.omegaBlueChestplate = true;
			modPlayer.noLifeRegen = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 16);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 8);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 8);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
