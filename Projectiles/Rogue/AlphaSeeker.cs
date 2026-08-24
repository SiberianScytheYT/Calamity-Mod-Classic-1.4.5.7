using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
	public class AlphaSeeker : ModProjectile
    {
        public static float moveSpeed = 2f;
        public static float rotateSpeed = 0.04f;
        public static int lifetime = 120;
        public static int returnTime = 60;
        public bool initialized = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seeker");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = lifetime;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (!initialized)
            {
                Projectile.rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                Projectile.localAI[1] = Main.rand.NextFloat(-rotateSpeed, rotateSpeed);
                initialized = true;
            }
            if (Projectile.ai[0] == 1f)
            {
                // Follow enemy
                float minDist = 999f;
                int index = 0;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(Projectile, false))
                    {
                        float dist = (Projectile.Center - npc.Center).Length();
                        if (dist < minDist)
                        {
                            minDist = dist;
                            index = i;
                        }
                    }
                }

                Vector2 velocityNew;
                if (minDist < 999f)
                {
                    velocityNew = Main.npc[index].Center - Projectile.Center;
                    velocityNew.Normalize();
                    Projectile.velocity += velocityNew;
                    if (Projectile.velocity.Length() > 10f)
                    {
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 10f;
                    }
                }
            }
            else if (Projectile.ai[0] == 2f)
            {
                // projectile.localAI[0] controls the distance from the parent projectile

                Projectile.tileCollide = false;

                Projectile parent = Main.projectile[0];
                bool active = false;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.identity == Projectile.ai[1] && p.active)
                    {
                        parent = p;
                        active = true;
                    }
                }

                if (active)
                {
                    Vector2 pos = new Vector2(0, Projectile.localAI[0]);
                    pos = pos.RotatedBy(Projectile.rotation);

                    Projectile.Center = parent.Center + pos;
                    Projectile.rotation += Projectile.localAI[1];

                    if (Projectile.timeLeft > returnTime)
                    {
                        Projectile.localAI[0] += moveSpeed;
                    }
                    else
                    {
                        Projectile.localAI[0] -= moveSpeed;
                    }
                }
                else
                {
                    Projectile.Kill();
                }
            }

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 89, 0f, 0f, 100, default, 2f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity.Y = -0.15f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 180);
        }
    }
}
