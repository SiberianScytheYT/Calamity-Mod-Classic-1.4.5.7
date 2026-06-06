using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class YharimsInsignia : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Yharim's Insignia");
/*
            Tooltip.SetDefault("10% increased damage when under 50% life\n" +
                "10% increased melee speed\n" +
                "10% increased melee and true melee damage\n" +
                "Melee attacks and melee projectiles inflict holy fire\n" +
                "Increased invincibility after taking damage\n" +
                "Temporary immunity to lava\n" +
                "Increased melee knockback");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip6")
					{
						line2.Text = "Increased melee knockback\n" +
						"Provides heat protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.yInsignia = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WarriorEmblem);
            recipe.AddIngredient(ModContent.ItemType<NecklaceofVexation>());
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 5);
			recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 5);
			recipe.AddIngredient(ItemID.CrossNecklace);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
