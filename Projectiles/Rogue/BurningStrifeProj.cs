using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class BurningStrifeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shadow Flame Spiky Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.timeLeft = 720;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.Calamity().rogue = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            //Rotation code
            Projectile.rotation += Projectile.velocity.X * 0.05f * Projectile.direction;
            //Gravity
            Projectile.velocity.Y += 0.05f;
            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;
            //Dust
            if (Projectile.ai[0] >= 25f)
            {
                Dust.NewDust(Projectile.Center, 1, 1, DustID.Shadowflame, -Projectile.velocity.X * 0.3f, -Projectile.velocity.Y * 0.3f, 0, default, 1.1f);
                Projectile.ai[0] = 0f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y * 0.7f;
            Projectile.velocity.X *= 0.9f;
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 180);
            if (Projectile.Calamity().stealthStrike && Projectile.penetrate != 1)
            {
                SoundEngine.PlaySound(SoundID.Item103, Projectile.Center);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowflameExplosionBig>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].Center = Projectile.Center;
                Main.projectile[proj].Calamity().rogue = true;
            }
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(BuffID.ShadowFlame, 180);
            if (Projectile.Calamity().stealthStrike && Projectile.penetrate != 1)
            {
                SoundEngine.PlaySound(SoundID.Item103, Projectile.Center);
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowflameExplosionBig>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].Center = Projectile.Center;
                Main.projectile[proj].Calamity().rogue = true;
            }
        }
        */

        public override void OnKill(int timeLeft)
        {
            int proj;
            SoundEngine.PlaySound(SoundID.Item103, Projectile.Center);
            if(Projectile.Calamity().stealthStrike)
                proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowflameExplosionBig>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner);
            else
                proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowflameExplosion>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner);
            Main.projectile[proj].Center = Projectile.Center;
            Main.projectile[proj].Calamity().rogue = true;
        }
    }
}
