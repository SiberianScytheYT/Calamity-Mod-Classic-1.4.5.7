using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Plates;
using CalRD.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class EldritchSoulArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eldritch Soul Artifact");
/*
            Tooltip.SetDefault("Knowledge\n" +
                "Boosts melee speed by 10%, ranged velocity by 25%, rogue damage by 15%, max minions by 2, and reduces mana cost by 15%");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
			Item.accessory = true;
			Item.rare = ItemRarityID.Red;
			Item.Calamity().customRarity = CalamityRarity.Turquoise;
			Item.value = CalamityGlobalItem.Rarity12BuyPrice;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.eArtifact = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 25);
			recipe.AddIngredient(ModContent.ItemType<Navyplate>(), 25);
			recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 5);
			recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
