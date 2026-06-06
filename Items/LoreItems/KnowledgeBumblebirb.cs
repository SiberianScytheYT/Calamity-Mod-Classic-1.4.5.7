using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeBumblebirb : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Dragonfolly");
/*
            Tooltip.SetDefault("A failure of twisted scientific ambition; it appears our faulted arrogance over life has shown once more in the results.\n" +
                "Originally intended to be a clone of the Jungle Dragon, these were left to roam about the jungle, attacking anything in their path.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 10;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<BumblebirbTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
