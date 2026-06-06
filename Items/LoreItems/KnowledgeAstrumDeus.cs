using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeAstrumDeus : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astrum Deus");
/*
            Tooltip.SetDefault("God of the stars and largest vessel for the Astral Infection.\n" +
                "Though struck down from its place among the stars its remnants have gathered strength, aiming to take its rightful place in the cosmos once more.\n" +
                "Favorite this item to gain increased movement speed in space.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 9;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
			if (Item.favorited)
				modPlayer.astrumDeusLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<AstrumDeusTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
