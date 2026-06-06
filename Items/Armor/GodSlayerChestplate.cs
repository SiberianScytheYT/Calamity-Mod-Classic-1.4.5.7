using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class GodSlayerChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("God Slayer Chestplate");
/*
            Tooltip.SetDefault("+60 max life\n" +
                       "15% increased movement speed\n" +
                       "Enemies take damage when they hit you\n" +
                       "Attacks have a 2% chance to do no damage to you\n" +
                       "11% increased damage and 6% increased critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.defense = 41;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.godSlayerReflect = true;
            player.thorns += 0.5f;
            player.statLifeMax2 += 60;
            player.moveSpeed += 0.15f;
            player.GetDamage(DamageClass.Generic) += 0.11f;
            modPlayer.AllCritBoost(6);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 23);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
