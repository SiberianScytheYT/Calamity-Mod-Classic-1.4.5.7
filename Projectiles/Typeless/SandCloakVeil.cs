using CalRD.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Typeless
{
	public class SandCloakVeil : ModProjectile
    {
        private const float radius = 225f;
        private const int duration = 900;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dust Veil");
        }

        public override void SetDefaults()
        {
            Projectile.width = 450;
            Projectile.height = 450;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = duration;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation += 0.01f;

            Player player = Main.player[Main.myPlayer];
            Vector2 posDiff = player.Center - Projectile.Center;
            if (posDiff.Length() <= radius)
            {
                player.statDefense += 6;
                player.lifeRegen += 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Sprite Circle
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            float scaleStep = 0.03f;
            float rotationOffset = 0.03f;
            Color drawCol = Projectile.GetAlpha(lightColor);
            float drawTransparency = 0.1f;

            if (Projectile.timeLeft > duration - 10)
            {
                drawTransparency = (duration - Projectile.timeLeft) * 0.01f;
            }
            else if (Projectile.timeLeft < 25)
            {
                drawTransparency = Projectile.timeLeft * 0.004f;
            }

            // Dust effects
            Circle dustCircle = new Circle(Projectile.Center, radius);

            for (int i = 0; i < 20; i++)
            {
                // Sprite
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, drawCol * drawTransparency, Projectile.rotation + (rotationOffset * i * i), tex.Size() / 2f, Projectile.scale - (i * scaleStep), SpriteEffects.None, 0f);

                // Dust
                Vector2 dustPos = dustCircle.RandomPointInCircle();
                if ((dustPos - Projectile.Center).Length() > 48)
                {
                    int dustIndex = Dust.NewDust(dustPos, 1, 1, 32);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].fadeIn = 1f;
                    Vector2 dustVelocity = Projectile.Center - Main.dust[dustIndex].position;
                    float distToCenter = dustVelocity.Length();
                    dustVelocity.Normalize();
                    dustVelocity = dustVelocity.RotatedBy(MathHelper.ToRadians(-90f));
                    dustVelocity *= distToCenter * 0.04f;
                    Main.dust[dustIndex].velocity = dustVelocity;
                }
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Knockback has to be done manually to ensure the enemies are repelled from the aura as opposed to thrown to one side of it

			if (target.knockBackResist <= 0f)
				return;

            if (CalamityGlobalNPC.ShouldAffectNPC(target))
            {
                float knockbackMultiplier = Projectile.knockBack - (1f - target.knockBackResist);
                if (knockbackMultiplier < 0)
                {
                    knockbackMultiplier = 0;
                }
                Vector2 trueKnockback = target.Center - Projectile.Center;
                trueKnockback.Normalize();
                target.velocity = trueKnockback * knockbackMultiplier;
            }
        }

        // Circular hitbox code copied from HeliumFlashBlast
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
