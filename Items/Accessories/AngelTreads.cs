using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    [AutoloadEquip(EquipType.Shoes)]
    public class AngelTreads : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Angel Treads");
/*
            Tooltip.SetDefault("Extreme speed!\n" +
                               "36% increased running acceleration\n" +
                               "Increased flight time\n" +
                               "Greater mobility on ice\n" +
                               "Water and lava walking\n" +
                               "Temporary immunity to lava");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 32;
            Item.value = CalamityGlobalItem.Rarity6BuyPrice;
            Item.rare = 6;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip5")
					{
						line2.Text = "Temporary immunity to lava\n" +
						"Provides heat protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.harpyRing = true;
            player.accRunSpeed = 8f;
            player.rocketBoots = 3;
            player.moveSpeed += 0.16f;
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaMax += 240;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FrostsparkBoots);
            recipe.AddIngredient(ItemID.LavaWaders);
            recipe.AddIngredient(ModContent.ItemType<HarpyRing>());
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 20);
            recipe.AddIngredient(ItemID.SoulofMight);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.AddIngredient(ItemID.SoulofFright);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
