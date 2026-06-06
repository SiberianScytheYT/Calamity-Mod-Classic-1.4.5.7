using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeYharon : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jungle Dragon, Yharon");
/*
            Tooltip.SetDefault("I would not be able to bear a world without my faithful companion by my side.\n" +
                "Fortunately, fate will have it so that it is a world I shall never have to see, for better or for worse.\n" +
                "Favorite this item to gain nearly-infinite wing flight time, but at the cost of a 25% decrease to all damage.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 10;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
			if (Item.favorited)
				modPlayer.yharonLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<YharonTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
