using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgePolterghast : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Polterghast");
/*
            Tooltip.SetDefault("A creature born of hatred and anger, formed by countless human souls with all of their energy entirely devoted to consuming others.\n" +
                "It seems a waste to have had such a potent source of power ravage mindlessly through these empty halls.\n" +
                "Favorite this item to gain increased item grab range.");
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
				modPlayer.polterghastLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<PolterghastTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
