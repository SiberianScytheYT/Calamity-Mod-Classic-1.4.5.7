using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeWallofFlesh : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wall of Flesh");
/*
            Tooltip.SetDefault("I see the deed is done.\n" +
                "The unholy amalgamation of flesh and hatred has been defeated.\n" +
                "Prepare to face the terrors that lurk in the light and dark parts of this world.\n" +
                "Favorite this item to gain increased item grab range.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 4;
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
				modPlayer.wallOfFleshLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.WallofFleshTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
