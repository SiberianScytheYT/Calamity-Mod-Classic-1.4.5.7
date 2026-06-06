using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeDestroyer : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Destroyer");
/*
            Tooltip.SetDefault("A machine brought to life by the mighty souls of warriors, and built to excavate massive tunnels in planets to gather resources.\n" +
                "Could have proven useful if Draedon didn't have an obsession with turning everything into a tool of destruction.\n" +
                "Favorite this item to gain a boost to your pick speed.\n" +
				"However, your max acceleration is decreased slightly due to you feeling heavier.");
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
				modPlayer.destroyerLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.DestroyerTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
