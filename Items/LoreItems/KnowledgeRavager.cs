using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeRavager : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ravager");
/*
            Tooltip.SetDefault("The flesh golem constructed using twisted necromancy during the time of my conquest to counter my unstoppable forces.\n" +
                "Its creators were slaughtered by it moments after its conception. It is for the best that it has been destroyed.\n" +
                "Favorite this item to gain an increase to all damage, but at the cost of reduced wing flight time.");
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
				modPlayer.ravagerLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<RavagerTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
