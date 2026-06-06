using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class CoreOfTheBloodGod : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Core of the Blood God");
/*
            Tooltip.SetDefault("8% increased damage and damage reduction\n" +
                "Boosts your max HP by 10%\n" +
                "Healing Potions are 15% more effective\n" +
                "Halves enemy contact damage\n" +
                "When you take contact damage this effect has a 20 second cooldown");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.expert = true;
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.coreOfTheBloodGod = true;
            modPlayer.fleshTotem = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodyWormScarf>());
            recipe.AddIngredient(ModContent.ItemType<BloodPact>());
            recipe.AddIngredient(ModContent.ItemType<FleshTotem>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
