using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeDevourerofGods : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Devourer of Gods");
/*
            Tooltip.SetDefault("This serpent�s power to assimilate the abilities and energy of those it consumed is unique in almost all the known cosmos, save for its lesser brethren.\n" +
                "I would have soon had to eliminate it as a threat had it been given more time and creatures to feast upon.\n" +
                "Favorite this item to boost the power of your true melee strikes by 25%.\n" +
				"However, due to your reckless nature you will take 5% increased damage.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 10;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
			if (Item.favorited)
				modPlayer.DoGLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<DevourerofGodsTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
