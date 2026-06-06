using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeOldDuke : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Old Duke");
/*
            Tooltip.SetDefault("Strange, to find out that the mutant terror of the seas was not alone in its unique biology.\n" +
                "Perhaps I was mistaken to classify the creature from its relation to pigrons alone.\n" +
                "Favorite this item to convert negative effects from the Acid Rain debuff to positive effects.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 10;
            Item.Calamity().postMoonLordRarity = 13;
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
				modPlayer.boomerDukeLore = true;
		}

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<OldDukeTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
