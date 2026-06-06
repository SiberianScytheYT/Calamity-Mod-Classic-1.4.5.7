using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeAstralInfection : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Infection");
/*
            Tooltip.SetDefault("This twisted dreamscape, surrounded by unnatural pillars under a dark and hazy sky.\n" +
                "Natural law has been upturned. What will you make of it?");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 9;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<AstrumDeusTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
