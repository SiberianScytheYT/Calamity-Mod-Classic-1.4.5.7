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
    public class BloodflareHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodflare Wyvern Helm");
/*
            Tooltip.SetDefault("You can move freely through liquids and have temporary immunity to lava\n" +
                "+3 max minions");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.defense = 16; //85
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
					{
						line2.Text = "+3 max minions\n" +
						"Provides heat protection in Death Mode";
					}
				}
			}
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BloodflareBodyArmor>() && legs.type == ModContent.ItemType<BloodflareCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.bloodflareSet = true;
            modPlayer.bloodflareSummon = true;
            player.setBonus = "55% increased minion damage\n" +
                "Greatly increases life regen\n" +
                "Enemies below 50% life have a chance to drop hearts when struck\n" +
                "Enemies above 50% life have a chance to drop mana stars when struck\n" +
                "Enemies killed during a Blood Moon have a much higher chance to drop Blood Orbs\n" +
                "Summons polterghast mines to circle you\n" +
                "At 90% life and above you gain 10% increased minion damage\n" +
                "At 50% life and below you gain 20 defense and 2 life regen";
            player.crimsonRegen = true;
            player.GetDamage(DamageClass.Summon) += 0.55f;
        }

        public override void UpdateEquip(Player player)
        {
            player.lavaMax += 240;
            player.ignoreWater = true;
            player.maxMinions += 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 11);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 2);
			recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
