using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Plates;
using CalRD.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class GodlySoulArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Godly Soul Artifact");
/*
            Tooltip.SetDefault("Loyalty\n" +
                "Summons two Sons of Yharon to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
			Item.accessory = true;
			Item.rare = ItemRarityID.Red;
			Item.Calamity().customRarity = CalamityRarity.Violet;
			Item.value = CalamityGlobalItem.Rarity15BuyPrice;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.gArtifact = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 25);
			recipe.AddIngredient(ModContent.ItemType<PlagueContainmentCells>(), 25);
			recipe.AddIngredient(ModContent.ItemType<HellcasterFragment>(), 5);
			recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
