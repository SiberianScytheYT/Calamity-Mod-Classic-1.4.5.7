using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.LoreItems
{
    public class KnowledgeCrabulon : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crabulon");
/*
            Tooltip.SetDefault("A crab and its mushrooms, a love story.\n" +
                "It's interesting how creatures can adapt given certain circumstances.\n" +
                "Favorite this item to gain the Mushy buff while underground or in the mushroom biome.\n" +
				"However, your movement speed will be decreased while in these areas due to you being covered in fungi.");
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
				modPlayer.crabulonLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<CrabulonTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
