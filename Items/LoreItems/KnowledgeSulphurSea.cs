using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeSulphurSea : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphur Sea");
/*
            Tooltip.SetDefault("I remember the serene waves and the clear breeze.\n" +
                "The bitterness of my youth has long since subsided, but it is far too late. I must never repeat a mistake like this again.");
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
            r.AddIngredient(ModContent.ItemType<AquaticScourgeTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
