using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee.Yoyos
{
	public class SulphurousGrabberYoyo : ModProjectile
    {
		private int bubbleCounter = 0;
		private bool bubbleStronk = false;
		private int bubbleStronkCounter = 0;
		private float arbitraryTimer = 0f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sulphurous Grabber Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 350f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 16f;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 99;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
			if (Projectile.owner == Main.myPlayer)
			{
				if (bubbleStronk)
				{
					ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 20f;
					Projectile.extraUpdates = 2;
					Projectile.usesLocalNPCImmunity = true;
					Projectile.localNPCHitCooldown = 10 * Projectile.extraUpdates;
					bubbleStronkCounter++;
				}
				else
				{
					ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 16f;
					Projectile.extraUpdates = 1;
					Projectile.usesLocalNPCImmunity = false;
					bubbleStronkCounter = 0;
				}

				if (bubbleStronkCounter >= 240)
					bubbleStronk = false;

				Rectangle rectangle = new Rectangle((int)((double)Projectile.position.X + (double)Projectile.velocity.X * 0.5 - 4.0), (int)((double)Projectile.position.Y + (double)Projectile.velocity.Y * 0.5 - 4.0), Projectile.width + 8, Projectile.height + 8);
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.type == ModContent.ProjectileType<SulphurousGrabberBubble2>() && proj.ai[0] >= 40f && proj.owner == Projectile.owner)
					{
						Rectangle rect = proj.getRect();
						if (rectangle.Intersects(rect))
						{
							proj.Kill();
							bubbleStronk = true;
							bubbleStronkCounter = 0;
							break;
						}
					}
				}

				arbitraryTimer += bubbleStronk ? 0.5f : 1f;

				bubbleCounter++;
				if (bubbleCounter >= 60)
				{
					int bubbleAmt = 7;
					for (float i = 0; i < bubbleAmt; i++)
					{
						int projType = ModContent.ProjectileType<SulphurousGrabberBubble>();
						if (Main.rand.NextBool(10))
							projType = ModContent.ProjectileType<SulphurousGrabberBubble2>();
						float angle = MathHelper.TwoPi / bubbleAmt * i + (float)Math.Sin(arbitraryTimer / 20f) * MathHelper.PiOver2;
						Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, angle.ToRotationVector2() * 8f, projType, Projectile.damage / 4, Projectile.knockBack / 4, Projectile.owner, 0f, 0f);
					}
					bubbleCounter = 0;
				}
			}

			if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
				Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            if (bubbleStronk)
            {
				tex = ModContent.Request<Texture2D>("CalRD/Projectiles/Melee/Yoyos/SulphurousGrabberYoyoBubble").Value;
                CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1, tex);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 120);
        }
    }
}
