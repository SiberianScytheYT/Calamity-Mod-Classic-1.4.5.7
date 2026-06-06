using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
    public class DemonBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Devil Fork");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.alpha = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
			Projectile.timeLeft = 300;
        }

        public override void AI()
        {
			int dustType = Utils.SelectRandom(Main.rand, new int[]
			{
				173,
				(int)CalamityDusts.Brimstone,
				172
			});
            if (Projectile.position.HasNaNs())
            {
                Projectile.Kill();
                return;
            }
            bool tileCheck = WorldGen.SolidTile(Framing.GetTileSafely((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16));
            Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default, 1f)];
            dust.position = Projectile.Center;
            dust.velocity = Vector2.Zero;
            dust.noGravity = true;
            if (tileCheck)
            {
                dust.noLight = true;
            }
            if (Projectile.ai[1] == -1f)
            {
                Projectile.ai[0] += 1f;
                Projectile.velocity = Vector2.Zero;
                Projectile.penetrate = -1;
                Projectile.position = Projectile.Center;
                Projectile.width = Projectile.height = 140;
                Projectile.Center = Projectile.position;
                Projectile.alpha -= 10;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                if (Projectile.ai[0] >= Projectile.MaxUpdates * 3)
                {
                    Projectile.Kill();
                }
                return;
            }
			else
				Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(45);
            if (Projectile.numUpdates == 0)
            {
                int targetIndex = -1;
                float detectRange = 60f;
                for (int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++)
                {
                    NPC npc = Main.npc[npcIndex];
                    if (npc.CanBeChasedBy(Projectile, false))
                    {
                        float npcDist = Projectile.Distance(npc.Center);
                        if (npcDist < detectRange && Collision.CanHitLine(Projectile.Center, 0, 0, npc.Center, 0, 0))
                        {
                            detectRange = npcDist;
                            targetIndex = npcIndex;
                        }
                    }
                }
                if (targetIndex != -1)
                {
                    Projectile.ai[0] = 0f;
                    Projectile.ai[1] = -1f;
                    Projectile.netUpdate = true;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            bool tileCheck = WorldGen.SolidTile(Framing.GetTileSafely((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16));
            for (int m = 0; m < 4; m++)
            {
				int dustType = Utils.SelectRandom(Main.rand, new int[]
				{
					173,
					(int)CalamityDusts.Brimstone,
					172
				});
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 1.5f);
            }
            for (int n = 0; n < 4; n++)
            {
				int dustType = Utils.SelectRandom(Main.rand, new int[]
				{
					173,
					(int)CalamityDusts.Brimstone,
					172
				});
                int dustInt = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default, 2.5f);
                Main.dust[dustInt].noGravity = true;
                Main.dust[dustInt].velocity *= 3f;
                if (tileCheck)
                {
                    Main.dust[dustInt].noLight = true;
                }
                dustInt = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 1.5f);
                Main.dust[dustInt].velocity *= 2f;
                Main.dust[dustInt].noGravity = true;
                if (tileCheck)
                {
                    Main.dust[dustInt].noLight = true;
                }
            }
        }

		//glowmask effect
        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor)
        {
			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            Projectile.Kill();
        }
    }
}
