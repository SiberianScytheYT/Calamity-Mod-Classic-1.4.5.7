using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgePlantera : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plantera");
/*
            Tooltip.SetDefault("Well done, you killed a plant.\n" +
                "It was used as a vessel to house the spirits of those unfortunate enough to find their way down here.\n" +
                "I wish you luck in dealing with the fallout.\n" +
				"Favorite this item to gain increased item grab range.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 6;
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
				modPlayer.planteraLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.PlanteraTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
