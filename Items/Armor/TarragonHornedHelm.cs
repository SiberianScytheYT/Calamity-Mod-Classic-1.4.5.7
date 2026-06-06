using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TarragonHornedHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tarragon Horned Helm");
/*
            Tooltip.SetDefault("Temporary immunity to lava\n" +
                "Can move freely through liquids\n" +
                "5% increased damage reduction and +3 max minions");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.defense = 3; //98
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip2")
					{
						line2.Text = "5% increased damage reduction and +3 max minions\n" +
						"Provides heat protection in Death Mode";
					}
				}
			}
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TarragonBreastplate>() && legs.type == ModContent.ItemType<TarragonLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.tarraSummon = true;
            player.setBonus = "55% increased minion damage\n" +
                "Reduces enemy spawn rates\n" +
                "Increased heart pickup range\n" +
                "Enemies have a chance to drop extra hearts on death\n" +
                "Summons a life aura around you that damages nearby enemies";
            player.GetDamage(DamageClass.Summon) += 0.55f;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 3;
            player.endurance += 0.05f;
            player.lavaMax += 240;
            player.ignoreWater = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 7);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
