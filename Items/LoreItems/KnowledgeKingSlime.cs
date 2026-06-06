using CalRD.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.LoreItems
{
    public class KnowledgeKingSlime : LoreItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("King Slime");
/*
            Tooltip.SetDefault("Only a fool could be caught by this pitiful excuse for a hunter.\n" +
                "Unfortunately, our world has no shortage of those.\n" +
				"Favorite this item to gain 5% increased movement speed and 2% increased jump speed.\n" +
				"However, your defense is reduced by 3 due to your gelatinous body.");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 1;
            Item.consumable = false;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            bool autoJump = Main.player[Main.myPlayer].autoJump;
			string hasJumpBoost = "Favorite this item to gain 5% increased movement speed and 2% increased jump speed.";
			string noJumpBoost = "Favorite this item to gain 5% increased movement speed.";
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip2")
                {
                    line2.Text = autoJump ? noJumpBoost : hasJumpBoost;
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void UpdateInventory(Player player)
        {
			if (Item.favorited)
				player.Calamity().kingSlimeLore = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddTile(TileID.Bookcases);
            r.AddIngredient(ItemID.KingSlimeTrophy);
            r.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            r.Register();
        }
    }
}
