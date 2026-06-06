using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee.Yoyos
{
	public class ShimmersparkYoyo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shimmerspark");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 16f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 350f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 17f;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 99;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
			Projectile.MaxUpdates = 2;
		}

        public override void AI()
        {
			CalamityGlobalProjectile.MagnetSphereHitscan(Projectile, 300f, 6f, 120f, 5, ProjectileID.HallowStar);
			if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
				Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 120);
        }
    }
}
