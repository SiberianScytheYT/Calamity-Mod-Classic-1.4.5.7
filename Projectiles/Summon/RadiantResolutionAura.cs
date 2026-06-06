using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Summon
{
    public class RadiantResolutionAura : ModProjectile
    {
        public const float DistanceToCheck = 1600f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Saros Possession");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 66;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 2f, 2f, 2f);
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            player.AddBuff(ModContent.BuffType<RadiantResolutionBuff>(), 3600);
            bool isCorrectProjectile = Projectile.type == ModContent.ProjectileType<RadiantResolutionAura>();
            if (isCorrectProjectile)
            {
                if (player.dead)
                {
                    modPlayer.radiantResolution = false;
                }
                if (modPlayer.radiantResolution)
                {
                    Projectile.timeLeft = 2;
                }
            }

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.Calamity().spawnedPlayerMinionDamageValue = player.MinionDamage();
                Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                Projectile.localAI[0] += 1f;
            }
            if (player.MinionDamage() != Projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int trueDamage = (int)(Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue /
                    Projectile.Calamity().spawnedPlayerMinionDamageValue *
                    player.MinionDamage());
                Projectile.damage = trueDamage;
            }
            Projectile.minionSlots = Projectile.ai[0];

            Projectile.Center = player.Center - Vector2.UnitY * 16f;

            float allocatedSlots = Projectile.ai[0];
            Projectile.rotation += MathHelper.ToRadians(3f) + MathHelper.ToRadians(allocatedSlots * 0.85f);

			float damageMult = (float)Math.Log(allocatedSlots, 3) + 1f;

			//Softcap the mult after 9 slots
			float newMult = damageMult;
			if (newMult > 3f)
			{
				newMult = ((damageMult - 3f) * 0.1f) + 3f;
			}

            int radiantOrbDamage = (int)(Projectile.damage * newMult);
            int radiantOrbAppearRate = (int)(130 * Math.Pow(0.9, allocatedSlots));

            if (radiantOrbAppearRate < 7)
                radiantOrbAppearRate = 7;

            if (radiantOrbDamage > 10000)
                radiantOrbDamage = 10000;

            Projectile.ai[1]++;
            NPC potentialTarget = Projectile.Center.MinionHoming(DistanceToCheck, player);
            if (potentialTarget != null && Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[1] % 35 == 34)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float angle = MathHelper.Lerp(-MathHelper.ToRadians(20f), MathHelper.ToRadians(20f), i / 2f);
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(potentialTarget.Center).RotatedBy(angle) * 15f,
                            ModContent.ProjectileType<RadiantResolutionFire>(), radiantOrbDamage / 2, Projectile.knockBack, Projectile.owner);
                    }
                }
                if (Projectile.ai[1] % radiantOrbAppearRate == radiantOrbAppearRate - 1)
                {
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center + Utils.NextVector2Unit(Main.rand) * Main.rand.NextFloat(100f, 360f),
                        Projectile.DirectionTo(potentialTarget.Center) * 2f, ModContent.ProjectileType<RadiantResolutionOrb>(), radiantOrbDamage, Projectile.knockBack * 4f, Projectile.owner);
                    for (int i = 0; i < 3; i++)
                    {
                        float angle = MathHelper.Lerp(-MathHelper.ToRadians(30f), MathHelper.ToRadians(30f), i / 3f);
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(potentialTarget.Center).RotatedBy(angle) * 19f,
                            ModContent.ProjectileType<RadiantResolutionFire>(), radiantOrbDamage / 2, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
        }

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */ => false;
        public override void PostDraw(Color lightColor)
        {
            Texture2D currentTexture = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(currentTexture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + MathHelper.PiOver2,
                currentTexture.Size() / 2f,
                1f,
                SpriteEffects.None,
                0f);
        }
    }
}
