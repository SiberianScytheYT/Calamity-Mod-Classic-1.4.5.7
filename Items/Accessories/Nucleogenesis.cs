using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class Nucleogenesis : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nucleogenesis");
/*
            Tooltip.SetDefault("Increased max minions by 4 and 15% increased minion damage\n" +
                "Increased minion knockback\n" +
                "Minions inflict a variety of debuffs\n" +
                "Minions spawn damaging sparks on enemy hits"); //subject to change to be "cooler"
*/            
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.nucleogenesis = true;
            modPlayer.shadowMinions = true; //shadowflame
            modPlayer.tearMinions = true; //temporal sadness
            modPlayer.voltaicJelly = true; //electrified
            modPlayer.starTaintedGenerator = true; //astral infection and irradiated
            player.GetKnockback(DamageClass.Summon).Base += 3f;
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.maxMinions += 4;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StarTaintedGenerator>());
            recipe.AddIngredient(ModContent.ItemType<StatisCurse>());
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
