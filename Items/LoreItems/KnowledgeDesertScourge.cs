using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeDesertScourge : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Desert Scourge");
/*
            Tooltip.SetDefault("The great sea worm appears to have survived the extreme heat and has even adapted to it.\n" +
                "What used to be a majestic beast swimming through the water has now become a dried-up and\n" +
                "gluttonous husk, constantly on a voracious search for its next meal.\n" +
                "Favorite this item for an increase to defense while in the desert or sunken sea.\n" +
				"However, you will deal decreased damage while in these areas due to being a husk of your former self.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 1;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

		public override void UpdateInventory(Player player)
		{
			if (Item.favorited)
				player.Calamity().desertScourgeLore = true;
		}

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<DesertScourgeTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
