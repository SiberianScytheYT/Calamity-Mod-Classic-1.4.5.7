using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRD.NPCs.StormWeaver;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Projectiles.Rogue
{
	public class AlphaVirusProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/AlphaVirus";

        public static int lifetime = 600;
        public static float finalVelocity = 2f;
        public static float decelerationRate = 0.07f;
        private const float radius = 100f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Alpha Virus");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = lifetime;
            Projectile.Calamity().rogue = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Projectile.rotation += 0.05f * Projectile.direction;

            if (Projectile.Calamity().stealthStrike)
            {
                if (Projectile.ai[0] > finalVelocity)
                {
                    Projectile.ai[0] -= decelerationRate;
                    if (Projectile.ai[0] < finalVelocity)
                    {
                        Projectile.ai[0] = finalVelocity;
                    }

                    Projectile.velocity.Normalize();
                    Projectile.velocity *= Projectile.ai[0];
                }
                if (Projectile.timeLeft < lifetime - 30 && Projectile.timeLeft % 15 == 0 && Projectile.ai[0] <= finalVelocity)
                {
                    int projdamage = Projectile.damage / 2;
                    Vector2 randomVelocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                    randomVelocity.Normalize();
                    randomVelocity *= 5f;

                    int p = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, randomVelocity, ModContent.ProjectileType<AlphaSeeker>(), projdamage, 1f, Projectile.owner, 2, Projectile.identity);
                }
            }
            else
            {
                Projectile.timeLeft--;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            float dist1 = Vector2.Distance(Projectile.Center, target.Hitbox.TopLeft());
            float dist2 = Vector2.Distance(Projectile.Center, target.Hitbox.TopRight());
            float dist3 = Vector2.Distance(Projectile.Center, target.Hitbox.BottomLeft());
            float dist4 = Vector2.Distance(Projectile.Center, target.Hitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            if (minDist <= Projectile.width)
            {
                target.AddBuff(ModContent.BuffType<Plague>(), 120);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            float dist1 = Vector2.Distance(Projectile.Center, target.Hitbox.TopLeft());
            float dist2 = Vector2.Distance(Projectile.Center, target.Hitbox.TopRight());
            float dist3 = Vector2.Distance(Projectile.Center, target.Hitbox.BottomLeft());
            float dist4 = Vector2.Distance(Projectile.Center, target.Hitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            if (minDist <= Projectile.width)
            {
                target.AddBuff(ModContent.BuffType<Plague>(), 120);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			if (target.type == ModContent.NPCType<StormWeaverHeadNaked>() || target.type == ModContent.NPCType<StormWeaverBodyNaked>() || target.type == ModContent.NPCType<StormWeaverTailNaked>())
            {
                modifiers.SourceDamage *= 0.2f;
            }

            float dist1 = Vector2.Distance(Projectile.Center, target.Hitbox.TopLeft());
            float dist2 = Vector2.Distance(Projectile.Center, target.Hitbox.TopRight());
            float dist3 = Vector2.Distance(Projectile.Center, target.Hitbox.BottomLeft());
            float dist4 = Vector2.Distance(Projectile.Center, target.Hitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            if (minDist > Projectile.width)
            {
                Projectile.damage /= 5;
                Projectile.knockBack = 0f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Aura effect
            Texture2D aura = ModContent.Request<Texture2D>("CalRD/Projectiles/Rogue/AlphaVirusAura").Value;
            float scaleStep = 0.03f;
            float rotationOffset = 0.03f;
            float drawTransparency = 0.1f;

            if (Projectile.timeLeft > lifetime - 10)
            {
                drawTransparency = (lifetime - Projectile.timeLeft) * (drawTransparency / 10);
            }
            else if (Projectile.timeLeft < 25)
            {
                drawTransparency = Projectile.timeLeft * (drawTransparency / 25);
            }

            Color drawCol = Color.White;

            for (int i = 0; i < 10; i++)
            {
                Main.spriteBatch.Draw(aura, Projectile.Center - Main.screenPosition, null, drawCol * drawTransparency, -(Projectile.rotation * 0.2f) + (rotationOffset * i * i), aura.Size() / 2f, Projectile.scale - (i * scaleStep), SpriteEffects.None, 0f);
            }

            // Dust
            for (int i = 0; i < (lifetime - Projectile.timeLeft) / 30; i++)
            {
                float min = Projectile.width / 2;
                float max = radius;

                Vector2 pos = new Vector2(0f, -Main.rand.NextFloat(min, max));
                pos = pos.RotatedByRandom(MathHelper.TwoPi);
                Vector2 velocity = -pos * 0.02f;
                pos += Projectile.Center;

                int dust = Dust.NewDust(pos, 1, 1, 89, 0f, 0f, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = velocity + Projectile.velocity;
            }

            // Main sprite
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Projectile.Kill();
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                int damage2 = Projectile.damage;
                Vector2 velocity = new Vector2(0, 10);
                velocity = velocity.RotatedBy(MathHelper.ToRadians(60) * i);
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<AlphaSeeker>(), damage2, 5, Projectile.owner, i % 2, 0);
            }

            int numDust = 20;
            for (int i = 0; i < numDust; i++)
            {
                float min = Projectile.width / 2;
                float max = radius;

                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat(min, max));
                velocity = velocity.RotatedByRandom(MathHelper.TwoPi);
                velocity *= 0.1f;

                int dust = Dust.NewDust(Projectile.Center, 1, 1, 89, 0f, 0f, 100, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = velocity + Projectile.velocity;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float dist1 = Vector2.Distance(Projectile.Center, targetHitbox.TopLeft());
            float dist2 = Vector2.Distance(Projectile.Center, targetHitbox.TopRight());
            float dist3 = Vector2.Distance(Projectile.Center, targetHitbox.BottomLeft());
            float dist4 = Vector2.Distance(Projectile.Center, targetHitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            return minDist <= radius;
        }
    }
}
