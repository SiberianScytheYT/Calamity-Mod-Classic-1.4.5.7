using CalRD.CalPlayer;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Typeless
{
    public class GladiatorSword2 : ModProjectile
    {
        private double rotation = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gladiator Sword");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft *= 5;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10 -
                (Main.hardMode ? 2 : 0) -
                (NPC.downedPlantBoss ? 2 : 0) -
                (NPC.downedMoonlord ? 2 : 0) -
                (CalamityWorld.downedDoG ? 2 : 0);
        }

        public override void AI()
        {
			Projectile.friendly = true;
			Projectile.hostile = false;
            bool flag64 = Projectile.type == ModContent.ProjectileType<GladiatorSword2>();
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.Calamity().spawnedPlayerMinionDamageValue = player.AverageDamage();
                Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                Projectile.localAI[0] += 1f;
            }
            if (player.AverageDamage() != Projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int damage2 = (int)((float)Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue /
                    Projectile.Calamity().spawnedPlayerMinionDamageValue *
                    player.AverageDamage());
                Projectile.damage = damage2;
            }
            if (!modPlayer.gladiatorSword)
            {
                Projectile.active = false;
                return;
            }
            if (flag64)
            {
                if (player.dead)
                {
                    modPlayer.glSword = false;
                }
                if (modPlayer.glSword)
                {
                    Projectile.timeLeft = 2;
                }
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.01f / 255f);
            Vector2 vector = player.Center - Projectile.Center;
            Projectile.rotation = vector.ToRotation() - 1.57f;
            Projectile.Center = player.Center + new Vector2(80, 0).RotatedBy(rotation);
			double rotateAmt = CalamityWorld.downedDoG ? 0.09 : NPC.downedMoonlord ? 0.07 : NPC.downedPlantBoss ? 0.04 : Main.hardMode ? 0.03 : 0.02;
			//values are slightly different from the other sword to make this sword marginally slower so the intersection point isn't always at the same spot
            rotation -= rotateAmt;
            if (rotation <= 0)
            {
                rotation = 360;
            }
            Projectile.velocity.X = (vector.X > 0f) ? -0.000001f : 0f;
        }
    }
}
