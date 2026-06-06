using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ElementalGauntlet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elemental Gauntlet");
/*
            Tooltip.SetDefault("Melee attacks and projectiles inflict most debuffs\n" +
                "15% increased melee speed, damage, and 5% increased melee critical strike chance\n" +
				"20% increased true melee damage\n" +
                "Increased invincibility after taking damage\n" +
                "Temporary immunity to lava\n" +
                "Increased melee knockback");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip6")
					{
						line2.Text = "Melee attacks have a chance to instantly kill normal enemies\n" +
						"Provides heat and cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.eGauntlet = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FireGauntlet);
            recipe.AddIngredient(ModContent.ItemType<YharimsInsignia>());
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
