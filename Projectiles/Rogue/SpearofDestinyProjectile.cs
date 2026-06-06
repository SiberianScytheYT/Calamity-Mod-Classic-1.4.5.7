using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class SpearofDestinyProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/SpearofDestiny";

		private bool initialized = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spear");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.aiStyle = 113;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.BoneJavelin;
            Projectile.Calamity().rogue = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
			if (!initialized)
			{
				if (Projectile.Calamity().stealthStrike)
				{
					Projectile.penetrate++;
					Projectile.tileCollide = false;
				}
				initialized = true;
			}
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 246, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

			Vector2 center = Projectile.Center;
			float maxDistance = 300f;
			bool homeIn = false;

			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].CanBeChasedBy(Projectile, false))
				{
					float extraDistance = (float)(Main.npc[i].width / 2) + (float)(Main.npc[i].height / 2);
					bool canHit = Projectile.Calamity().stealthStrike || Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1);

					if (Vector2.Distance(Main.npc[i].Center, Projectile.Center) < (maxDistance + extraDistance) && canHit)
					{
						center = Main.npc[i].Center;
						homeIn = true;
						break;
					}
				}
			}

			if (homeIn)
			{
                Projectile.extraUpdates = 2;
				Vector2 homeInVector = Projectile.DirectionTo(center);
				if (homeInVector.HasNaNs())
					homeInVector = Vector2.UnitY;

				Projectile.velocity = (Projectile.velocity * 20f + homeInVector * 12f) / (21f);
			}
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 246, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 120);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
            target.AddBuff(BuffID.Ichor, 120);
        }*/

		public override void PostDraw(Color lightColor)
		{
			if (Projectile.Calamity().stealthStrike)
			{
				Texture2D tex = ModContent.Request<Texture2D>("CalRD/Projectiles/Rogue/SpearofDestinyGlow").Value;
				Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
			}
		}
    }
}
