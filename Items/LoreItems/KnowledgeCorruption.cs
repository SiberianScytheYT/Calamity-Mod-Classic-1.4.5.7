using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeCorruption : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Corruption");
/*
            Tooltip.SetDefault("The rotten and forever-deteriorating landscape of infected life, brought upon by a deadly microbe long ago.\n" +
                "It is rumored that the microbe was created through experimentation by a long-dead race, predating the Terrarians.\n" +
                "Favorite this item to prevent hive cysts from spawning.");
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
				modPlayer.corruptionLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.EaterofWorldsTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
