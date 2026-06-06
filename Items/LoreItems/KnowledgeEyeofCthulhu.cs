using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeEyeofCthulhu : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Eye of Cthulhu");
/*
            Tooltip.SetDefault("That eye...how peculiar.\n" +
                "I sensed it watching you more intensely as you grew stronger.\n" +
                "Favorite this item for night vision at night.\n" +
				"However, your vision is reduced during the day.");
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
			{
				if (!Main.dayTime)
					player.nightVision = true;
				else
					player.blind = true;
			}
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.EyeofCthulhuTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
