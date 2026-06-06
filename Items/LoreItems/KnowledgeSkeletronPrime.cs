using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeSkeletronPrime : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Skeletron Prime");
/*
            Tooltip.SetDefault("What a silly and pointless contraption for something created with the essence of pure terror.\n" +
                "Draedon obviously took several liberties with its design...I am not impressed.\n" +
                "Favorite this item to gain a boost to your armor penetration.\n" +
				"However, your max acceleration is decreased due to you feeling heavier.");
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
				modPlayer.skeletronPrimeLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.SkeletronPrimeTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
