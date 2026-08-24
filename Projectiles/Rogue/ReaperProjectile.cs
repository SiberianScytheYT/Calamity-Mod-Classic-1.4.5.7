using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
	public class ReaperProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/TheReaper";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scythe");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 12;
            Projectile.timeLeft = 750;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= 60f)
                {
                    Projectile.localAI[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
                else
                {
					CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 400f, 20f, 20f);
                }
            }
            else
            {
                float num633 = 700f;
                bool flag24 = false;
                if (Projectile.ai[0] == 1f)
                {
                    Projectile.ai[1] += 1f;
                    if (Projectile.ai[1] > 40f)
                    {
                        Projectile.ai[1] = 1f;
                        Projectile.ai[0] = 0f;
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        flag24 = true;
                    }
                }
                if (flag24)
                {
                    return;
                }
                Vector2 vector46 = Projectile.position;
                bool flag25 = false;
                for (int num645 = 0; num645 < Main.maxNPCs; num645++)
                {
                    NPC nPC2 = Main.npc[num645];
                    if (nPC2.CanBeChasedBy(Projectile, false))
                    {
                        float num646 = Vector2.Distance(nPC2.Center, Projectile.Center);
                        if (!flag25)
                        {
                            num633 = num646;
                            vector46 = nPC2.Center;
                            flag25 = true;
                        }
                    }
                }
                if (flag25 && Projectile.ai[0] == 0f)
                {
                    Vector2 vector47 = vector46 - Projectile.Center;
                    float num648 = vector47.Length();
                    vector47.Normalize();
                    if (num648 > 200f)
                    {
                        float scaleFactor2 = 8f;
                        vector47 *= scaleFactor2;
                        Projectile.velocity = (Projectile.velocity * 40f + vector47) / 41f;
                    }
                    else
                    {
                        float num649 = 4f;
                        vector47 *= -num649;
                        Projectile.velocity = (Projectile.velocity * 40f + vector47) / 41f;
                    }
                }
                if (Projectile.ai[1] > 0f)
                {
                    Projectile.ai[1] += (float)Main.rand.Next(1, 4);
                }
                if (Projectile.ai[1] > 40f)
                {
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
                if (Projectile.ai[0] == 0f)
                {
                    if (Projectile.ai[1] == 0f && flag25 && num633 < 500f)
                    {
                        Projectile.ai[1] += 1f;
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Projectile.ai[0] = 1f;
                            Vector2 value20 = vector46 - Projectile.Center;
                            value20.Normalize();
                            Projectile.velocity = value20 * 8f;
                            Projectile.netUpdate = true;
                        }
                    }
                }
            }
            Projectile.rotation += 0.5f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 600);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 600);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int num621 = 0; num621 < 5; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 8; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
        }
    }
}
