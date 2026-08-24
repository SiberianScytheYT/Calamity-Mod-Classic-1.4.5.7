using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Hybrid
{
	public class EradicatorProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/Eradicator";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eradicator");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 180;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.35f, 0f, 0.25f);
			CalamityGlobalProjectile.MagnetSphereHitscan(Projectile, 300f, 6f, 8f, 6, ModContent.ProjectileType<NebulaShot>(), 1D, true);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Vector2 origin = new Vector2(31f, 29f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/EradicatorGlow").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 600);
            target.AddBuff(BuffID.Frostburn, 600);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 600);
            target.AddBuff(BuffID.Frostburn, 600);
        }
    }
}
