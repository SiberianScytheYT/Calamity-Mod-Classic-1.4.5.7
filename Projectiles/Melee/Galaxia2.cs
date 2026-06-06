using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Buffs.Potions;
using CalRD.CalPlayer;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Melee
{
    public class Galaxia2 : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Melee/Galaxia";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orb");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 50;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 20 + Main.rand.Next(40);
                if (Main.rand.NextBool(5))
                {
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }
            Projectile.alpha -= 15;
            int num58 = 150;
            if (Projectile.Center.Y >= Projectile.ai[1])
            {
                num58 = 0;
            }
            if (Projectile.alpha < num58)
            {
                Projectile.alpha = num58;
            }
            Projectile.localAI[0] += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * (float)Projectile.direction;
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * (float)Projectile.direction;
            if (Main.rand.NextBool(8))
            {
                Vector2 value3 = Vector2.UnitX.RotatedByRandom(1.5707963705062866).RotatedBy((double)Projectile.velocity.ToRotation(), default);
                int num59 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.2f);
                Main.dust[num59].velocity = value3 * 0.66f;
                Main.dust[num59].noGravity = true;
                Main.dust[num59].position = Projectile.Center + value3 * 12f;
            }
            if (Main.rand.NextBool(24))
            {
                int num60 = Gore.NewGore(Entity.GetSource_FromThis(), Projectile.Center, new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f), 16, 1f);
                Main.gore[num60].velocity *= 0.66f;
                Main.gore[num60].velocity += Projectile.velocity * 0.3f;
            }
            if (Projectile.ai[1] == 1f)
            {
                Projectile.light = 0.9f;
                if (Main.rand.NextBool(5))
                {
                    int num59 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.2f);
                    Main.dust[num59].noGravity = true;
                }
                if (Main.rand.NextBool(10))
                {
                    Gore.NewGore(Entity.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
                }
            }

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 1600f, 35f, 20f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
			CalamityPlayer modPlayer = player.Calamity();
			bool astral = modPlayer.ZoneAstral;
            bool jungle = player.ZoneJungle;
            bool snow = player.ZoneSnow;
            bool beach = player.ZoneBeach;
            bool corrupt = player.ZoneCorrupt;
            bool crimson = player.ZoneCrimson;
            bool dungeon = player.ZoneDungeon;
            bool desert = player.ZoneDesert;
            bool glow = player.ZoneGlowshroom;
            bool hell = player.ZoneUnderworldHeight;
            bool holy = player.ZoneHallow;
            bool nebula = player.ZoneTowerNebula;
            bool stardust = player.ZoneTowerStardust;
            bool solar = player.ZoneTowerSolar;
            bool vortex = player.ZoneTowerVortex;
            bool bloodMoon = Main.bloodMoon;
            bool snowMoon = Main.snowMoon;
            bool pumpkinMoon = Main.pumpkinMoon;
            if (bloodMoon)
            {
                player.AddBuff(BuffID.Battle, 600);
            }
            if (snowMoon)
            {
                player.AddBuff(BuffID.RapidHealing, 600);
            }
            if (pumpkinMoon)
            {
                player.AddBuff(BuffID.WellFed, 600);
            }
            if (astral)
			{
                target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 1200);
                player.AddBuff(ModContent.BuffType<GravityNormalizerBuff>(), 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<AstralStar>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
			}
			else if (jungle)
            {
                target.AddBuff(ModContent.BuffType<Plague>(), 1200);
                player.AddBuff(BuffID.Thorns, 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.Leaf, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
            else if (snow)
            {
                target.AddBuff(ModContent.BuffType<GlacialState>(), 1200);
                player.AddBuff(BuffID.Warmth, 600);
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.IceBolt, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
            else if (beach)
            {
                target.AddBuff(ModContent.BuffType<CrushDepth>(), 1200);
                player.AddBuff(BuffID.Wet, 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, ProjectileID.FlaironBubble, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
            else if (corrupt)
            {
                player.AddBuff(BuffID.Wrath, 600);
                int ball = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.CursedFlameFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[ball].Calamity().forceMelee = true;
                Main.projectile[ball].penetrate = 1;
            }
            else if (crimson)
            {
                player.AddBuff(BuffID.Rage, 600);
                int ball = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.GoldenShowerFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[ball].Calamity().forceMelee = true;
                Main.projectile[ball].penetrate = 1;
            }
            else if (dungeon)
            {
                target.AddBuff(BuffID.Frostburn, 1200);
                player.AddBuff(BuffID.Dangersense, 600);
                int ball = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.WaterBolt, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[ball].Calamity().forceMelee = true;
                Main.projectile[ball].penetrate = 1;
            }
            else if (desert)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 1200);
                player.AddBuff(BuffID.Endurance, 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.BlackBolt, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
            else if (glow)
            {
                target.AddBuff(ModContent.BuffType<TemporalSadness>(), 1200);
                player.AddBuff(BuffID.Spelunker, 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.Mushroom, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
            else if (hell)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 1200);
                player.AddBuff(BuffID.Inferno, 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.BallofFire, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
            else if (holy)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 1200);
                player.AddBuff(BuffID.Heartreach, 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.RainbowCrystalExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
				Main.projectile[proj].usesLocalNPCImmunity = true;
				Main.projectile[proj].localNPCHitCooldown = -1;
            }
            else if (nebula)
            {
                player.AddBuff(BuffID.MagicPower, 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.NebulaBlaze1, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
            else if (stardust)
            {
                player.AddBuff(BuffID.Summoning, 600);
                int ball = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.StardustCellMinionShot, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[ball].Calamity().forceMelee = true;
                Main.projectile[ball].penetrate = 1;
            }
            else if (solar)
            {
                player.AddBuff(BuffID.Titan, 600);
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.SolarWhipSwordExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
            }
            else if (vortex)
            {
                player.AddBuff(BuffID.AmmoReservation, 600);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.VortexBeaterRocket, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
				Main.projectile[proj].usesLocalNPCImmunity = true;
				Main.projectile[proj].localNPCHitCooldown = -1;
            }
            else
            {
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 1200);
                player.AddBuff(BuffID.DryadsWard, 600);
                int ball = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileID.TerrarianBeam, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[ball].penetrate = 1;
            }
        }
    }
}
