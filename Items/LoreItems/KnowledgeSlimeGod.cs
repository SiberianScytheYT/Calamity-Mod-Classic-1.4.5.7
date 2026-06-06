using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeSlimeGod : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Slime God");
/*
            Tooltip.SetDefault("It is a travesty, one of the most threatening biological terrors ever created.\n" +
                "If this creature were allowed to combine every slime on the planet it would become nearly unstoppable.\n" +
                "Favorite this item to become slimed and able to slide around on tiles, at the cost of reduced defense.\n" +
                "This effect does not work with mounts.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 4;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (player.mount.Active || !Item.favorited)
                return;

            modPlayer.slimeGodLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ModContent.ItemType<SlimeGodTrophy>());
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
