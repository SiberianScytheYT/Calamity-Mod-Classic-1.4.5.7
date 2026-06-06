using CalRD.CalPlayer;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AmalgamatedBrain : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Amalgamated Brain");
/*
            Tooltip.SetDefault("10% increased damage\n" +
                               "Shade rains down when you are hit\n" +
                               "You will confuse nearby enemies when you are struck");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.expert = true;
            Item.rare = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.aBrain = true;
            if (player.immune)
            {
                if (player.miscCounter % 6 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
						Projectile rain = CalamityUtils.ProjectileRain(player.GetSource_FromThis(), player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<AuraRain>(), (int)(60 * player.AverageDamage()), 2f, player.whoAmI, 6, 1);
						rain.tileCollide = false;
						rain.penetrate = 1;
                    }
                }
            }
            player.GetDamage(DamageClass.Generic) += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RottenBrain>());
            recipe.AddIngredient(ItemID.BrainOfConfusion);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
