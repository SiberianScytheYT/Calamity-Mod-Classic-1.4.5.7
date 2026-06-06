using CalRD.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
    public class EternityHoming : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eternity");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            NPC target = null;
            float distance = 4400f;
            for (int index = 0; index < Main.npc.Length; index++)
            {
                if (Main.npc[index].CanBeChasedBy(null, false))
                {
                    if (Main.npc[index].boss && Vector2.Distance(Projectile.Center, Main.npc[index].Center) <= 4400f)
                    {
                        target = Main.npc[index];
                        break;
                    }
                    if (Vector2.Distance(Projectile.Center, Main.npc[index].Center) < distance)
                    {
                        distance = Vector2.Distance(Projectile.Center, Main.npc[index].Center);
                        target = Main.npc[index];
                    }
                }
            }
            if (target != null)
            {
                Projectile.velocity = (Projectile.velocity * 7f + Projectile.DirectionTo(target.Center) * 10f) / 8f;
            }
            Projectile.ai[0] += 0.18f;
            float angle = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            float pulse = (float)Math.Sin(Projectile.ai[0]);
            float radius = 10f;
            Vector2 offset = angle.ToRotationVector2() * pulse * radius;
            Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, Eternity.DustID, Vector2.Zero, 0, Eternity.BlueColor);
            dust.noGravity = true;

            dust = Dust.NewDustPerfect(Projectile.Center - offset, Eternity.DustID, Vector2.Zero, 0, Eternity.BlueColor);
            dust.noGravity = true;
        }
    }
}