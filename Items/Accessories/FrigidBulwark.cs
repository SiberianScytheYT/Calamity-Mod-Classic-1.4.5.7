using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class FrigidBulwark : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frigid Bulwark");
/*
            Tooltip.SetDefault("Absorbs 25% of damage done to players on your team\n" +
                "Only active above 25% life\n" +
                "Grants immunity to knockback\n" +
                "Puts a shell around the owner when below 50% life that reduces damage\n" +
                "The shell becomes more powerful when below 15% life and reduces damage even further");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 44;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.defense = 8;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip4")
					{
						line2.Text = "The shell becomes more powerful when below 15% life and reduces damage even further\n" +
						"Provides heat and cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.fBulwark = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PaladinsShield);
            recipe.AddIngredient(ItemID.FrozenTurtleShell);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
