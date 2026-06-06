using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgePlaguebringerGoliath : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Plaguebringer Goliath");
/*
            Tooltip.SetDefault("A horrific amalgam of steel, flesh, and infection, capable of destroying an entire civilization in just one onslaught.\n" +
                "Its plague nuke barrage can leave an entire area uninhabitable for months. A shame that it came to this but the plague must be contained.\n" +
                "Favorite this item to gain increased wing flight time, but at the cost of reduced life regen.");
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
				modPlayer.plaguebringerGoliathLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<PlaguebringerGoliathTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
