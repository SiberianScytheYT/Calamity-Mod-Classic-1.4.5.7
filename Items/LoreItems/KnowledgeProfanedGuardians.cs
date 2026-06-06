using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeProfanedGuardians : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Profaned Guardians");
/*
            Tooltip.SetDefault("The ever-rejuvenating guardians of the profaned flame.\n" +
                "Much like a phoenix from the ashes their deaths are simply a part of their life cycle.\n" +
                "Many times my forces have had to destroy these beings in search of the Profaned Goddess.");
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
            r.AddIngredient(ModContent.ItemType<ProfanedGuardianTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
