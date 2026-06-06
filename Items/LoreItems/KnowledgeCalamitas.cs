using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeCalamitas : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Calamitas");
/*
            Tooltip.SetDefault("The witch unrivaled. Perhaps the only one amongst my cohorts to have ever given me cause for doubt.\n" +
                "Now that you have defeated her your destiny is clear.\n" +
                "Come now, face me.\n" +
                "Favorite this item to die instantly from every hit.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 10;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
			if (Item.favorited)
				modPlayer.SCalLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<SupremeCalamitasTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
