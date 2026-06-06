using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Plates;
using CalRD.Items.Placeables.Ores;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ProfanedSoulArtifact : ModItem //My precious babs <3 ~Amber
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Profaned Soul Artifact");
/*
            Tooltip.SetDefault("Purity\n" +
                "Summons a healer guardian which heals for a certain amount of health every few seconds\n" +
                "Summons a defensive guardian if you have at least 10 minion slots, which boosts your movement speed and your damage resistance\n" +
                "Summons an offensive guardian if you are wearing the tarragon summon set (or stronger), which boosts your summon damage and your minion slots\n" +
                "If you get hit, most of their effects will disappear for 5 seconds");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 40;
			Item.accessory = true;
			Item.rare = ItemRarityID.Red;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
			Item.value = CalamityGlobalItem.Rarity12BuyPrice;
		}

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */
        {
            return !player.Calamity().pArtifact;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.pArtifact = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 25);
			recipe.AddIngredient(ModContent.ItemType<Chaosplate>(), 25);
			recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 5);
			recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
