using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Weapons.Rogue;

namespace CalRD.Projectiles.Rogue
{
	public class Honeycomb : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/HardenedHoneycomb";

        private const float radius = 15f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Honeycomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 2;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];
            Vector2 posDiff = player.Center - Projectile.Center;
            if (posDiff.Length() <= radius)
            {
				player.AddBuff(BuffID.Honey, 300);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			int split = 0;
			while (split < 3)
			{
				//Calculate the velocity of the projectile
				float shardspeedX = -Projectile.velocity.X * Main.rand.NextFloat(.5f, .7f) + Main.rand.NextFloat(-3f, 3f);
				float shardspeedY = -Projectile.velocity.Y * Main.rand.Next(50, 70) * 0.01f + Main.rand.Next(-8, 9) * 0.2f;
				//Prevents the projectile speed from being too low
				if (shardspeedX < 2f && shardspeedX > -2f)
				{
					shardspeedX += -Projectile.velocity.X;
				}
				if (shardspeedY > 2f && shardspeedY < 2f)
				{
					shardspeedY += -Projectile.velocity.Y;
				}

				//Spawn the projectile
				Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.position.X + shardspeedX, Projectile.position.Y + shardspeedY, shardspeedX, shardspeedY, ModContent.ProjectileType<HoneycombFragment>(), (int)(Projectile.damage * 0.3), 2f, Projectile.owner, Main.rand.Next(3), 0f);
				split += 1;
			}
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.velocity.X = -Projectile.velocity.X;
			Projectile.velocity.Y = -Projectile.velocity.Y;
			int split = 0;
			while (split < 3)
			{
				//Calculate the velocity of the projectile
				float shardspeedX = -Projectile.velocity.X * Main.rand.NextFloat(.5f, .7f) + Main.rand.NextFloat(-3f, 3f);
				float shardspeedY = -Projectile.velocity.Y * Main.rand.Next(50, 70) * 0.01f + Main.rand.Next(-8, 9) * 0.2f;
				//Prevents the projectile speed from being too low
				if (shardspeedX < 2f && shardspeedX > -2f)
				{
					shardspeedX += -Projectile.velocity.X;
				}
				if (shardspeedY > 2f && shardspeedY < 2f)
				{
					shardspeedY += -Projectile.velocity.Y;
				}

				//Spawn the projectile
				Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.position.X + shardspeedX, Projectile.position.Y + shardspeedY, shardspeedX, shardspeedY, ModContent.ProjectileType<HoneycombFragment>(), (int)(Projectile.damage * 0.3), 2f, Projectile.owner, Main.rand.Next(3), 0f);
				split += 1;
			}
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
			Projectile.velocity.X = -Projectile.velocity.X;
			Projectile.velocity.Y = -Projectile.velocity.Y;
			int split = 0;
			while (split < 3)
			{
				//Calculate the velocity of the projectile
				float shardspeedX = -Projectile.velocity.X * Main.rand.NextFloat(.5f, .7f) + Main.rand.NextFloat(-3f, 3f);
				float shardspeedY = -Projectile.velocity.Y * Main.rand.Next(50, 70) * 0.01f + Main.rand.Next(-8, 9) * 0.2f;
				//Prevents the projectile speed from being too low
				if (shardspeedX < 2f && shardspeedX > -2f)
				{
					shardspeedX += -Projectile.velocity.X;
				}
				if (shardspeedY > 2f && shardspeedY < 2f)
				{
					shardspeedY += -Projectile.velocity.Y;
				}

				//Spawn the projectile
				Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.position.X + shardspeedX, Projectile.position.Y + shardspeedY, shardspeedX, shardspeedY, ModContent.ProjectileType<HoneycombFragment>(), (int)(Projectile.damage * 0.3), 2f, Projectile.owner, Main.rand.Next(3), 0f);
				split += 1;
			}
        }
        */

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
            //Dust on impact
            int dust_splash = 0;
            while (dust_splash < 9)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 9, -Projectile.velocity.X * 0.15f, -Projectile.velocity.Y * 0.15f, 159, default, 1.5f);
                dust_splash += 1;
            }
            if (Main.rand.NextBool(2))
            {
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<HardenedHoneycomb>());
            }
        }
    }
}
