using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class DestructionStar : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/StarofDestruction";

		public int hitCount = 0;
        private static float Radius = 47f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Star of Destruction");
        }

        public override void SetDefaults()
        {
            Projectile.width = 94;
            Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.penetrate = 16;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 191, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * MathHelper.ToRadians(8f);
			if (Projectile.Calamity().stealthStrike || hitCount > 16)
				hitCount = 16;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount++;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            hitCount++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
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

            return minDist <= Radius;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            Vector2 vector2 = new Vector2(20f, 20f);
			for (int index1 = 0; index1 < 10; ++index1)
			{
				int index2 = Dust.NewDust(Projectile.Center - vector2 / 2f, (int) vector2.X, (int) vector2.Y, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
				Dust dust = Main.dust[index2];
				dust.velocity = dust.velocity * 1.4f;
			}
            if (Projectile.owner == Main.myPlayer)
            {
				for (int i = 0; i < hitCount; i++)
				{
					Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<DestructionBolt>(), (int)(Projectile.damage * 0.5), 0f, Main.myPlayer, 0f, 0f);
                }
			}
		}
    }
}
