using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using CalRD.Projectiles.Ranged;
namespace CalRD.Projectiles.Rogue
{
    public class Prismalline3 : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/Prismalline";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Prismalline");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 300f, 25f, 20f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 154, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
			if (Projectile.ai[0] == 1f)
			{
				int shardCount = Main.rand.Next(1,4);
				for (int s = 0; s < shardCount; s++)
				{
					Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
					int shard = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<AquashardSplit>(), Projectile.damage / 3, 0f, Projectile.owner, 0f, 0f);
					Main.projectile[shard].Calamity().forceRogue = true;
					Main.projectile[shard].penetrate = 1;
				}
			}
        }
    }
}
