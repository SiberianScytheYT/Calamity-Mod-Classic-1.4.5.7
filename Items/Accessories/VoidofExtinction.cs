using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Projectiles.Typeless;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class VoidofExtinction : ModItem
    {
        public const int FireProjectiles = 2;
        public const float FireAngleSpread = 120;
        public int FireCountdown = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Void of Extinction");
/*
            Tooltip.SetDefault("No longer cursed\n" +
                "Drops brimstone fireballs from the sky occasionally\n" +
                "15% increase to all damage\n" +
                "Brimstone fire rains down while invincibility is active\n" +
                "Temporary immunity to lava, greatly reduces lava burn damage, and 25% increased damage while in lava");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.expert = true;
            Item.rare = 8;
            Item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip4")
					{
						line2.Text = "Temporary immunity to lava, greatly reduces lava burn damage, and 25% increased damage while in lava\n" +
						"Provides heat protection in Death Mode";
					}
				}
			}
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */ => !player.Calamity().calamityRing;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ObsidianRose);
            recipe.AddIngredient(ModContent.ItemType<Gehenna>());
            recipe.AddIngredient(ModContent.ItemType<CalamityRing>());
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>());
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.calamityRing = true;
			modPlayer.voidOfExtinction = true;
            player.lavaRose = true;
            player.lavaMax += 240;
            player.GetDamage(DamageClass.Generic) += 0.15f;
            if (player.lavaWet)
            {
                player.GetDamage(DamageClass.Generic) += 0.25f;
            }
            if (player.immune)
            {
                if (player.miscCounter % 10 == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
						CalamityUtils.ProjectileRain(player.GetSource_FromThis(), player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<StandingFire>(), (int)(30 * player.AverageDamage()), 5f, player.whoAmI, 0, 1, 60);
                    }
                }
            }
            if (FireCountdown == 0)
            {
                FireCountdown = 600;
            }
            if (FireCountdown > 0)
            {
                FireCountdown--;
                if (FireCountdown == 0)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        int projSpeed = 25;
                        float spawnX = Main.rand.Next(1000) - 500 + player.Center.X;
                        float spawnY = -1000 + player.Center.Y;
                        Vector2 baseSpawn = new Vector2(spawnX, spawnY);
                        Vector2 baseVelocity = player.Center - baseSpawn;
                        baseVelocity.Normalize();
                        baseVelocity *= projSpeed;
                        for (int i = 0; i < FireProjectiles; i++)
                        {
                            Vector2 spawn = baseSpawn;
                            spawn.X += i * 30 - (FireProjectiles * 15);
                            Vector2 velocity = baseVelocity.RotatedBy(MathHelper.ToRadians(-FireAngleSpread / 2 + (FireAngleSpread * i / (float)FireProjectiles)));
                            velocity.X = velocity.X + 3 * Main.rand.NextFloat() - 1.5f;
                            Projectile.NewProjectile(player.GetSource_Accessory(Item), spawn, velocity, ModContent.ProjectileType<BrimstoneHellfireballFriendly2>(), (int)(70 * player.AverageDamage()), 5f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
            }
        }
    }
}
