using CalRD.CalPlayer;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class RottenBrain : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rotten Brain");
/*
            Tooltip.SetDefault("10% increased damage when below 75% life\n"
                               +"5% decreased movement speed when below 50% life\n"
                               +"Shade rains down when you are hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.expert = true;
            Item.rare = 3;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.immune)
            {
                if (player.miscCounter % 6 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
						Projectile rain = CalamityUtils.ProjectileRain(player.GetSource_FromThis(), player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<AuraRain>(), (int)(18 * player.AverageDamage()), 2f, player.whoAmI, 6, 1);
						rain.tileCollide = false;
						rain.penetrate = 1;
                    }
                }
            }
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.rBrain = true;
        }
    }
}
