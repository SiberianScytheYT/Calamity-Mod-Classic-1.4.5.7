using CalRD.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class ToothBallProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/ToothBall";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.aiStyle = 2;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.ThrowingKnife;
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
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<ToothBall>());
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Projectile.owner == Main.myPlayer && Projectile.Calamity().stealthStrike)
			{
				Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, -10f, ModContent.ProjectileType<StealthNimbusCloud>(), Projectile.damage / 2, 0f, Projectile.owner, 1f, 0f);
			}
        }
    }
}
