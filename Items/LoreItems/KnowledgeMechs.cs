using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeMechs : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Mechanical Bosses");
/*
            Tooltip.SetDefault("I see you have awakened Draedon's old toys.\n" +
                "Once useful tools turned into savage beasts when their AIs went rogue, a mistake that Draedon failed to rectify in time.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 5;
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
            r.AddIngredient(ItemID.RetinazerTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();

            r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.SpazmatismTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();

            r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.DestroyerTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();

            r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.SkeletronPrimeTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
