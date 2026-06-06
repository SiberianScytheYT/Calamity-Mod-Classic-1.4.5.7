using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class SignusScythe : ModProjectile
    {
        private int counter = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scythe");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 420;
            Projectile.alpha = 100;
            Projectile.penetrate = -1;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(counter);
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            counter = reader.ReadInt32();
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
            Projectile.rotation += 0.5f * Projectile.direction;

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.Item73, Projectile.position);
            }

            int num469 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1f);
            Main.dust[num469].noGravity = true;
            Main.dust[num469].velocity *= 0f;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 150f)
            {
                if (Projectile.ai[1] > 0f && Projectile.ai[0] < 160f)
                {
                    int num625 = (int)Projectile.ai[1] - 1;
                    if (num625 < Main.maxPlayers)
                    {
                        Vector2 value16 = Main.player[num625].Center - Projectile.Center;
                        Projectile.velocity = Vector2.Normalize(value16) * 22f;
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft > 515)
            {
                Projectile.localAI[1] += 1f;
                byte b2 = (byte)(((int)Projectile.localAI[1]) * 3);
                byte a2 = (byte)(Projectile.alpha * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(255, 255, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 300);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
            for (int num621 = 0; num621 < 5; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 10; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1.5f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 100, default, 1f);
                Main.dust[num624].velocity *= 2f;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
