using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeMoonLord : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moon Lord");
/*
            Tooltip.SetDefault("What a waste.\n" +
                "Had it been fully restored it would have been a force to behold, but what you fought was an empty shell.\n" +
                "However, that doesn't diminish the immense potential locked within it, released upon its death.\n" +
                "Favorite this item to gain an improved Gravity Globe that gives you an increase to all stats while upside down.\n" +
				"However, while not upside down you have permanent featherfall.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 10;
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
				modPlayer.moonLordLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.MoonLordTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
