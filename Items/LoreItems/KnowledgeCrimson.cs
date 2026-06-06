using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeCrimson : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Crimson");
/*
            Tooltip.SetDefault("This bloody hell, spawned from a formless mass of flesh that fell from the stars eons ago.\n" +
                "It is now home to many hideous creatures, spawned from the pumping blood and lurching organs deep within.\n" +
                "Favorite this item to prevent perforator cysts from spawning.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 2;
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
				modPlayer.crimsonLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.BrainofCthulhuTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
