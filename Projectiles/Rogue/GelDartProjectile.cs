using CalRD.Items.Weapons.Rogue;
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
    public class GelDartProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/GelDart";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dart");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.aiStyle = 2;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.Calamity().rogue = true;
        }

		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer && Projectile.Calamity().stealthStrike)
			{
				Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * (float)Projectile.direction;
				if (Projectile.timeLeft % 8 == 0)
				{
					Vector2 velocity = new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f));
					int flame = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ProjectileID.SlimeGun, (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.5f, Projectile.owner, 0f, 0f);
					Main.projectile[flame].Calamity().forceRogue = true;
					Main.projectile[flame].usesLocalNPCImmunity = true;
					Main.projectile[flame].localNPCHitCooldown = 10;
                }
			}
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
				if (Projectile.Calamity().stealthStrike)
				{
					if (Projectile.velocity != oldVelocity)
					{
						Projectile.velocity = Main.rand.NextFloat(-1.15f, -0.85f) * oldVelocity * 1.35f;
                        SoundEngine.PlaySound(SoundID.Item56, Projectile.position); // Minecart bumper sound
					}
				}
				else
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
				}
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(2))
            {
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<GelDart>());
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 120);
			if (Projectile.Calamity().stealthStrike)
				target.AddBuff(BuffID.Slow, 120);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(BuffID.Slimed, 120);
			if (Projectile.Calamity().stealthStrike)
				target.AddBuff(BuffID.Slow, 120);
        }
        */
    }
}
