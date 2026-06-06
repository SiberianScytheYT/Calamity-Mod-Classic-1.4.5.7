using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeDukeFishron : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Duke Fishron");
/*
            Tooltip.SetDefault("The mutant terror of the sea was once the trusted companion of an old king; he tamed it using its favorite treat.\n" +
                "Long ago, the creature flew in desperation from the raging bloody inferno consuming its home, ultimately finding its way to the ocean.\n" +
                "Favorite this item for an increase to all damage, crit and movement speed while submerged in liquid.\n" +
				"However, while not submerged in liquid you will have slightly decreased damage, crit and movement speed due to feeling dried out.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 8;
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
				modPlayer.dukeFishronLore = true;
		}

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.DukeFishronTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
