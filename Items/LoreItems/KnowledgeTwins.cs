using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeTwins : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Twins");
/*
            Tooltip.SetDefault("The bio-mechanical watchers of the night, originally created as security using the souls extracted from human eyes.\n" +
                "These creatures did not belong in this world, it's best to be rid of them.\n" +
                "Favorite this item to gain invisibility and rogue bonuses at night.\n" +
				"However, your defense is reduced while above 50% life due to you feeling softer.\n" +
				"Your max acceleration is reduced while below 50% life due to you feeling heavier.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 5;
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
				modPlayer.twinsLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.RetinazerTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();

            r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.SpazmatismTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
