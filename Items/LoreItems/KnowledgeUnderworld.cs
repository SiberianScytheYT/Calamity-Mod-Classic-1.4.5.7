using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeUnderworld : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Underworld");
/*
            Tooltip.SetDefault("These obsidian and hellstone towers were once home to thousands of...'people'.\n" +
                "Unfortunately for them, they were twisted by their inner demons until they were beyond saving.\n" +
                "Favorite this item to prevent voodoo demons from dropping voodoo dolls.");
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
				modPlayer.underworldLore = true;
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
