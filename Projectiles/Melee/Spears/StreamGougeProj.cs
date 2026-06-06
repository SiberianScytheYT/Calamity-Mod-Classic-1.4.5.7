using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using CalRD.Projectiles.BaseProjectiles;
namespace CalRD.Projectiles.Melee.Spears
{
    public class StreamGougeProj : BaseSpearProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gouge");
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            // Projectile.aiStyle = 19;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            //projectile.Calamity().trueMelee = true;
        }

        public override float InitialSpeed => 3f;
        public override float ForwardSpeed => 0.95f;
        public override float ReelbackSpeed => 2.4f;

        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/Projectiles/Melee/Spears/StreamGougeGlow").Value,
                             Projectile.Center - Main.screenPosition,
                             null,
                             Color.White,
                             Projectile.rotation,
                             Vector2.Zero,
                             1f,
                             SpriteEffects.None,
                             0f);
        }
        public override Action<Projectile> EffectBeforeReelback => (proj) =>
        {
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y,
                ModContent.ProjectileType<EssenceBeam>(), Projectile.damage * 4, Projectile.knockBack, Projectile.owner, 0f, 0f);
        };

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
        }
    }
}
