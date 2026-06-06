using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeSkeletron : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Skeletron");
/*
            Tooltip.SetDefault("The curse is said to only affect the elderly.\n" +
                "After they are afflicted they become an immortal vessel for an ancient demon of the underworld.\n" +
                "Favorite this item to gain increased damage while in the dungeon.\n" +
				"However, your max health is decreased due to Skeletron's curse.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 3;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (player.ZoneDungeon && Item.favorited)
            {
                CalamityPlayer modPlayer = player.Calamity();
                modPlayer.skeletronLore = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.SkeletronTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
