using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class SpearofPaleolithProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/SpearofPaleolith";

        private bool stealthInit = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spear");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 113;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.BoneJavelin;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            if (Projectile.Calamity().stealthStrike && !stealthInit)
            {
                Projectile.velocity *= 0.5f;
                Projectile.timeLeft = 1800;
                stealthInit = true;
            }

            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 159, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            if (Projectile.timeLeft % 3 == 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FossilShardThrown>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
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
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 159, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 120);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 120);
        }*/
    }
}
