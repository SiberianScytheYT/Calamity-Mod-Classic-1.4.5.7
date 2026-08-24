using CalRD.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class TitaniumShurikenProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/TitaniumShuriken";

        private static float RotationIncrement = 0.22f;
        private static float ReboundTime = 26f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shuriken");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 2;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.Calamity().rogue = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            if (Projectile.Calamity().stealthStrike)
            {
				DrawOffsetX = -11;
				DrawOriginOffsetY = -10;
				DrawOriginOffsetX = 0;

				// ai[0] stores whether the knife is returning. If 0, it isn't. If 1, it is.
				if (Projectile.ai[0] == 0f)
				{
					Projectile.ai[1] += 1f;
					if (Projectile.ai[1] >= ReboundTime)
					{
						Projectile.ai[0] = 1f;
						Projectile.ai[1] = 0f;
						Projectile.netUpdate = true;
					}
				}
				else
				{
					Projectile.tileCollide = false;
					float returnSpeed = 16f;
					float acceleration = 3.2f;
					Player owner = Main.player[Projectile.owner];

					// Delete the shuriken if it's excessively far away.
					Vector2 playerCenter = owner.Center;
					float xDist = playerCenter.X - Projectile.Center.X;
					float yDist = playerCenter.Y - Projectile.Center.Y;
					float dist = (float)Math.Sqrt((double)(xDist * xDist + yDist * yDist));
					if (dist > 3000f)
						Projectile.Kill();

					dist = returnSpeed / dist;
					xDist *= dist;
					yDist *= dist;

					// Home back in on the player.
					if (Projectile.velocity.X < xDist)
					{
						Projectile.velocity.X = Projectile.velocity.X + acceleration;
						if (Projectile.velocity.X < 0f && xDist > 0f)
							Projectile.velocity.X += acceleration;
					}
					else if (Projectile.velocity.X > xDist)
					{
						Projectile.velocity.X = Projectile.velocity.X - acceleration;
						if (Projectile.velocity.X > 0f && xDist < 0f)
							Projectile.velocity.X -= acceleration;
					}
					if (Projectile.velocity.Y < yDist)
					{
						Projectile.velocity.Y = Projectile.velocity.Y + acceleration;
						if (Projectile.velocity.Y < 0f && yDist > 0f)
							Projectile.velocity.Y += acceleration;
					}
					else if (Projectile.velocity.Y > yDist)
					{
						Projectile.velocity.Y = Projectile.velocity.Y - acceleration;
						if (Projectile.velocity.Y > 0f && yDist < 0f)
							Projectile.velocity.Y -= acceleration;
					}

					// Delete the projectile if it touches its owner.
					if (Main.myPlayer == Projectile.owner)
						if (Projectile.Hitbox.Intersects(owner.Hitbox))
						{
							Projectile.Kill(); //boomerangs return to you so you get a refund
							Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<TitaniumShuriken>());
						}
				}

				// Rotate the shuriken as it flies.
				Projectile.rotation += RotationIncrement;
				return;
			}
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.Calamity().stealthStrike)
			{
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				return false;
			}
			return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.Calamity().stealthStrike)
            {
                CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            }
			else
			{
				Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
			}
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(2) && !Projectile.Calamity().stealthStrike)
            {
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<TitaniumShuriken>());
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Projectile.owner == Main.myPlayer && Projectile.Calamity().stealthStrike)
			{
				int projNumber = Main.rand.Next(1,3);
				for (int index2 = 0; index2 < projNumber; index2++)
				{
					float xVector = (float)Main.rand.Next(-35, 36) * 0.02f;
					float yVector = (float)Main.rand.Next(-35, 36) * 0.02f;
					xVector *= 10f;
					yVector *= 10f;
					Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, xVector, yVector, ModContent.ProjectileType<TitaniumClone>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
				}
			}
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if (Projectile.owner == Main.myPlayer && Projectile.Calamity().stealthStrike)
			{
				int projNumber = Main.rand.Next(1,3);
				for (int index2 = 0; index2 < projNumber; index2++)
				{
					float xVector = (float)Main.rand.Next(-35, 36) * 0.02f;
					float yVector = (float)Main.rand.Next(-35, 36) * 0.02f;
					xVector *= 10f;
					yVector *= 10f;
					Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, xVector, yVector, ModContent.ProjectileType<TitaniumClone>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
				}
			}
        }
    }
}
