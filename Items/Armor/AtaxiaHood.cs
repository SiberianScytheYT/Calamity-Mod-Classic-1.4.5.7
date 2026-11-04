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
    public class AtaxiaHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hydrothermic Hood");
/*
            Tooltip.SetDefault("12% increased rogue damage and 10% increased rogue critical strike chance\n" +
                "50% chance to not consume rogue items and 15% increased movement speed\n" +
                "Temporary immunity to lava and immunity to fire damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 8;
            Item.defense = 12; //49
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip2")
					{
						line2.Text += "\n" + CalamityUtils.GetTextValue("Misc.HeatProtection");
					}
				}
			}
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AtaxiaArmor>() && legs.type == ModContent.ItemType<AtaxiaSubligar>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
			player.Calamity().hydrothermalSmoke = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.AtaxiaHood");
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.ataxiaBlaze = true;
            modPlayer.ataxiaVolley = true;
            modPlayer.rogueStealthMax += 1.1f;
            modPlayer.throwingDamage += 0.05f;
            modPlayer.wearingRogueArmor = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingAmmoCost *= 0.5f;
            player.Calamity().throwingDamage += 0.12f;
            player.Calamity().throwingCrit += 10;
            player.moveSpeed += 0.15f;
            player.lavaMax += 240;
            player.buffImmune[BuffID.OnFire] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 7);
			recipe.AddIngredient(ItemID.HellstoneBar, 4);
			recipe.AddIngredient(ModContent.ItemType<CoreofChaos>());
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
