using CalRD.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class AdamantiteThrowingAxeProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/AdamantiteThrowingAxe";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Adamantite Throwing Axe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 2;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.Shuriken;
            Projectile.Calamity().rogue = true;
            Projectile.localNPCHitCooldown = 10;
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
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, Projectile.width, Projectile.height, ModContent.ItemType<AdamantiteThrowingAxe>());
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			OnHitEffects();
		}

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
			OnHitEffects();
		}
		*/

		private void OnHitEffects()
		{
			if (Projectile.Calamity().stealthStrike && Main.myPlayer == Projectile.owner)
			{
				for (int n = 0; n < 3; n++)
				{
					Projectile lightning = CalamityUtils.ProjectileRain(Projectile.GetSource_FromThis(), Projectile.Center, 400f, 100f, -800f, -500f, 8f, ModContent.ProjectileType<BlunderBoosterLightning>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
					lightning.ai[0] = Main.rand.Next(2);
				}
			}
		}
    }
}
