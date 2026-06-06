using CalRD.CalPlayer;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class RaidersTalisman : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Raider's Talisman");
/*
            Tooltip.SetDefault("Whenever you crit an enemy with a rogue weapon your rogue damage increases\n" +
                "This effect can stack up to 150 times\n" +
				"Max rogue damage boost is 15%\n" +
                "This line is modified below");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.rare = 2;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            int critLevel = Main.player[Main.myPlayer].Calamity().raiderStack;
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip3")
                {
                    line2.Text = "Rogue Crit Level: " + critLevel;
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.raiderTalisman = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Leather, 5);
            recipe.AddIngredient(ItemID.Obsidian, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
