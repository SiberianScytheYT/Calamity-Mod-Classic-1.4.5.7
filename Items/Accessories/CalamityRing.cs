using CalRD.CalPlayer;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class CalamityRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Void of Calamity");
/*
            Tooltip.SetDefault("Cursed? Reduces damage reduction by 10%\n" +
			"15% increase to all damage\n" +
			"Brimstone fire rains down while invincibility is active");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
            Item.expert = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */ => !player.Calamity().calamityRing;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.calamityRing = true;
            player.GetDamage(DamageClass.Generic) += 0.15f;
            player.endurance -= 0.1f;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.immune)
                {
                    if (player.miscCounter % 10 == 0)
                    {
						CalamityUtils.ProjectileRain(player.GetSource_FromThis(), player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<StandingFire>(), (int)(30 * player.AverageDamage()), 5f, player.whoAmI);
                    }
                }
            }
        }
    }
}
