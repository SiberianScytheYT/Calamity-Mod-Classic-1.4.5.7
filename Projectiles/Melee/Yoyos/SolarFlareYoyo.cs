using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee.Yoyos
{
    public class SolarFlareYoyo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Solar Flare");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 560f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 18f;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 99;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1.15f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 244, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
				Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<HolyExplosionSupreme>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[proj].Calamity().forceMelee = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
