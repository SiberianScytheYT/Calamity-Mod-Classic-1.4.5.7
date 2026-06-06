using CalRD.CalPlayer;
using CalRD.Buffs.DamageOverTime;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class ElysianAegis : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elysian Aegis");
/*
            Tooltip.SetDefault("Blessed by the Profaned Flame\n" +
							   "Grants immunity to fire blocks, knockback, and Holy Flames\n" +
                               "+40 max life and increased life regen\n" +
                               "Grants a supreme holy flame dash\n" +
                               "Can be used to ram enemies\n" +
                               "TOOLTIP LINE HERE\n" +
                               "Activating this buff will reduce your movement speed and increase enemy aggro");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 42;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.defense = 8;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string hotkey = CalRD.AegisHotKey.TooltipHotkeyString();
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip5")
                {
                    line2.Text = "Press " + hotkey + " to activate buffs to all damage, crit chance, and defense";
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
			modPlayer.dashMod = 3;
            modPlayer.elysianAegis = true;
            player.buffImmune[ModContent.BuffType<HolyFlames>()] = true;
            player.noKnockback = true;
            player.fireWalk = true;
            player.lifeRegen += 2;
            player.statLifeMax2 += 40;
        }
    }
}
