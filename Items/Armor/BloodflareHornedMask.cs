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
    public class BloodflareHornedMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodflare Hydra Hood");
/*
            Tooltip.SetDefault("You can move freely through liquids and have temporary immunity to lava\n" +
                "10% increased magic damage and critical strike chance, +100 max mana, and 17% reduced mana usage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.defense = 22; //85
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
						line2.Text = "10% increased magic damage and critical strike chance, +100 max mana, and 17% reduced mana usage\n" +
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
            modPlayer.bloodflareMage = true;
            player.setBonus = "Greatly increases life regen\n" +
                "Enemies below 50% life have a chance to drop hearts when struck\n" +
                "Enemies above 50% life have a chance to drop mana stars when struck\n" +
                "Enemies killed during a Blood Moon have a much higher chance to drop Blood Orbs\n" +
                "Magic weapons will sometimes fire ghostly bolts\n" +
                "Magic critical strikes cause flame explosions every 2 seconds";
            player.crimsonRegen = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.manaCost *= 0.83f;
            player.lavaMax += 240;
            player.ignoreWater = true;
            player.GetDamage(DamageClass.Magic) += 0.1f;
            player.GetCritChance(DamageClass.Magic) += 10;
            player.statManaMax2 += 100;
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
