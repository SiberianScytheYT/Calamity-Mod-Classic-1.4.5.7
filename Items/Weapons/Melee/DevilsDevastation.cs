using CalRD.Items.Materials;
using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class DevilsDevastation : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Devil's Devastation");
/*
            Tooltip.SetDefault("Fires a spread of demonic scythes\n" + "Pitchforks rise from the underworld to skewer your foes\n" +
                "Critical hits cause shadowflame explosions");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 118;
            Item.height = 118;
            Item.damage = 520;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<Oathblade>();
            Item.shootSpeed = 28f;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//Shoot 3 oathblades in a spread
			int index = 8;
            for (int j = -index; j <= index; j += index)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(j));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }

			//Not actually sure what this middle code does
            float speed = Item.shootSpeed;
            Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
            float directionX = (float)Main.mouseX + Main.screenPosition.X - source1.X;
            float directionY = (float)Main.mouseY + Main.screenPosition.Y - source1.Y;
            if (player.gravDir == -1f)
            {
                directionY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - source1.Y;
            }
			Vector2 direction = new Vector2(directionX, directionY);
            float aimDist = direction.Length();
            if ((float.IsNaN(direction.X) && float.IsNaN(direction.Y)) || (direction.X == 0f && direction.Y == 0f))
            {
                direction.X = (float)player.direction;
                direction.Y = 0f;
                aimDist = speed;
            }
            else
            {
                aimDist = speed / aimDist;
            }

			//Shoot 9 tridents from below
            int projAmt = 3; //Since the method spawns 3 projectiles, it spawns 9 total
            for (int projIndex = 0; projIndex < projAmt; projIndex++)
            {
                source1 = new Vector2(player.Center.X + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y + 600f);
                source1.X = (source1.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                source1.Y += (float)(100 * projIndex);
                direction.X = (float)Main.mouseX + Main.screenPosition.X - source1.X;
                direction.Y = (float)Main.mouseY + Main.screenPosition.Y - source1.Y;
                if (direction.Y < 0f)
                {
                    direction.Y *= -1f;
                }
                if (direction.Y < 20f)
                {
                    direction.Y = 20f;
                }
                aimDist = direction.Length();
                aimDist = speed / aimDist;
                direction.X *= aimDist;
                direction.Y *= aimDist;
                direction.X += Main.rand.NextFloat(-40f, 40f) * 0.02f;
                direction.Y += Main.rand.NextFloat(-40f, 40f) * 0.02f;
				direction.Y *= -1;
                Projectile.NewProjectile(source, source1, direction, ModContent.ProjectileType<DemonBlast>(), damage, Item.knockBack, player.whoAmI, 0f, Main.rand.Next(5));
                Projectile.NewProjectile(source, source1, direction, ModContent.ProjectileType<DemonBlast>(), damage, Item.knockBack, player.whoAmI, 0f, Main.rand.Next(3));
                Projectile.NewProjectile(source, source1, direction, ModContent.ProjectileType<DemonBlast>(), damage, Item.knockBack, player.whoAmI, 0f, 1f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Devastation>());
            recipe.AddIngredient(ModContent.ItemType<TrueForbiddenOathblade>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
            target.AddBuff(BuffID.OnFire, 300);
            if (hit.Crit)
            {
                target.AddBuff(BuffID.ShadowFlame, 900);
                target.AddBuff(BuffID.OnFire, 900);
                player.ApplyDamageToNPC(target, (int)(Item.damage * player.MeleeDamage()) * 4, 0f, 0, false);
                float scalar1 = 1.7f;
                float scalar2 = 0.8f;
                float scalar3 = 2f;
                Vector2 value3 = (target.rotation - MathHelper.PiOver2).ToRotationVector2();
                Vector2 value4 = value3 * target.velocity.Length();
                SoundEngine.PlaySound(SoundID.Item14, target.position);
                for (int i = 0; i < 40; i++)
                {
                    int dustInt = Dust.NewDust(target.position, target.width, target.height, 173, 0f, 0f, 200, default, scalar1);
                    Dust dust = Main.dust[dustInt];
                    dust.position = target.Center + Vector2.UnitY.RotatedByRandom(Math.PI) * Main.rand.NextFloat() * target.width / 2f;
                    dust.noGravity = true;
                    dust.velocity.Y -= 4.5f;
                    dust.velocity *= 3f;
                    dust.velocity += value4 * Main.rand.NextFloat();
                    dustInt = Dust.NewDust(target.position, target.width, target.height, 173, 0f, 0f, 100, default, scalar2);
                    dust.position = target.Center + Vector2.UnitY.RotatedByRandom(Math.PI) * Main.rand.NextFloat() * target.width / 2f;
                    dust.velocity.Y -= 3f;
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                    dust.color = Color.Crimson * 0.5f;
                    dust.velocity += value4 * Main.rand.NextFloat();
                }
                for (int j = 0; j < 20; j++)
                {
                    int dustInt = Dust.NewDust(target.position, target.width, target.height, 173, 0f, 0f, 0, default, scalar3);
                    Dust dust = Main.dust[dustInt];
                    dust.position = target.Center + Vector2.UnitX.RotatedByRandom(Math.PI).RotatedBy((double)target.velocity.ToRotation(), default) * target.width / 3f;
                    dust.noGravity = true;
                    dust.velocity.Y -= 1.5f;
                    dust.velocity *= 0.5f;
                    dust.velocity += value4 * (0.6f + 0.6f * Main.rand.NextFloat());
                }
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Shadowflame>(), 300);
            target.AddBuff(BuffID.OnFire, 300);
            // siberian's note: so i guess pvp hits just never worked... all that work for nothing, huh?
            /*
            if (crit)
            {
                target.AddBuff(ModContent.BuffType<Shadowflame>(), 900);
                target.AddBuff(BuffID.OnFire, 900);
                SoundEngine.PlaySound(SoundID.Item14, target.position);
            }
            */
        }
    }
}
