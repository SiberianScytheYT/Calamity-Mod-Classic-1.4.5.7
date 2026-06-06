using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class DemonshadeBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Demonshade Breastplate");
/*
            Tooltip.SetDefault("20% increased melee speed, 15% increased damage and critical strike chance\n" +
                       "Enemies take ungodly damage when they touch you\n" +
                       "Increased max life and mana by 200\n" +
                       "Standing still lets you absorb the shadows and boost your life regen");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(4, 0, 0, 0);
            Item.defense = 50;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.shadeRegen = true;
            player.thorns += 100f;
            player.statLifeMax2 += 200;
            player.statManaMax2 += 200;
            player.GetDamage(DamageClass.Generic) += 0.15f;
            modPlayer.AllCritBoost(15);
            player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 15);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
