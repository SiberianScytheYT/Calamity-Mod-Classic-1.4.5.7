using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
    public class AMRShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("AMR");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 10;
            Projectile.scale = 1.18f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			OnHitEffects(target.Center, hit.Crit);
            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 600);
            if (target.defense > 50)
            {
                target.defense -= 50;
            }
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			OnHitEffects(target.Center, false);
            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 600);
		}

		private void OnHitEffects(Vector2 targetPos, bool crit)
		{
            if (crit)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
						CalamityUtils.ProjectileBarrage(Projectile.GetSource_FromThis(), Projectile.Center, targetPos, x > 4, 500f, 500f, 0f, 500f, 10f, ModContent.ProjectileType<AMR2>(), (int)(Projectile.damage * 0.1), Projectile.knockBack * 0.1f, Projectile.owner);
					}
                }
            }
        }
    }
}
