using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeEaterofWorlds : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Eater of Worlds");
/*
            Tooltip.SetDefault("Perhaps it was just a giant worm infected by the microbe, given centuries to feed and grow its festering body.\n" +
                "Seems likely, given the origins of this place.\n" +
                "Deadly microbes spawn around you while this item is favorited.\n" +
				"However, you will have decreased life regen due to your skin rotting off.");
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
				modPlayer.eaterOfWorldsLore = true;
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
