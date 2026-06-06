using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRD.Buffs.DamageOverTime;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
    public class GalileosMoon : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Magic/Crescent";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crescent Moon");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0f, 0.6f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 20 + Main.rand.Next(40);
                if (Main.rand.NextBool(5))
                {
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }
			Projectile.rotation += Projectile.direction * 0.55f;
            CalamityGlobalProjectile.HomeInOnNPC(Projectile, true, 460f, 25f, 20f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 180);
        }
    }
}
