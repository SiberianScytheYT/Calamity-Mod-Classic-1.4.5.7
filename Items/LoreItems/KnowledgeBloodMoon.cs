using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeBloodMoon : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Red Moon");
/*
            Tooltip.SetDefault("We long ago feared the light of the red moon.\n" +
                "Many went mad, others died, but a scant few became blessed with a wealth of cosmic understanding.");
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
            r.AddIngredient(ItemID.AncientCultistTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
