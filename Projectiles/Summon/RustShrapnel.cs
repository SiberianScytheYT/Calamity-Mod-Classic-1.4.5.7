using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Summon
{
    public class RustShrapnel : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Rogue/BarrelShrapnel";

        public bool HitTile = false;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shrapnel");
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.minionSlots = 0f;
            Projectile.minion = true;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (HitTile)
            {
                Projectile.velocity.X = 0f;
                Projectile.rotation = MathHelper.Pi;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            Projectile.velocity.Y += 0.4f;
            Projectile.velocity.X *= 0.96f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            HitTile = true;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
            Projectile.Kill();
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.SulfurousSeaAcid);
                dust.velocity = Vector2.One.RotatedBy(i / 15f * MathHelper.TwoPi) * 3f * (float)Math.Cos(i / 15f * MathHelper.TwoPi);
                dust.scale = 2.5f;
                dust.noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
