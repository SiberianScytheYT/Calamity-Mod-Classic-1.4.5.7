using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeLunaticCultist : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lunatic Cultist");
/*
            Tooltip.SetDefault("The gifted one that terminated my grand summoning so long ago with his uncanny powers over the arcane.\n" +
                "Someone I once held in such contempt for his actions is now...deceased, his sealing ritual undone...prepare for the end.\n" +
                "Your impending doom approaches...\n" +
                "Favorite this item for an increase to all stats during the lunar event.\n" +
				"However, your vision is decreased due to eldritch knowledge damaging your mind.");
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

        public override void UpdateInventory(Player player)
        {
            if (NPC.LunarApocalypseIsUp && Item.favorited)
            {
                CalamityPlayer modPlayer = player.Calamity();
                modPlayer.lunaticCultistLore = true;
            }
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
