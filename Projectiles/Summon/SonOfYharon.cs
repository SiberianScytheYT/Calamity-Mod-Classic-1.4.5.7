using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Summon
{
    public class SonOfYharon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Son of Yharon");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.extraUpdates = 1;
            Projectile.minionSlots = 4f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.Calamity().spawnedPlayerMinionDamageValue = player.MinionDamage();
                Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                int num501 = 100;
                for (int num502 = 0; num502 < num501; num502++)
                {
                    int num503 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 16f), Projectile.width, Projectile.height - 16, 244, 0f, 0f, 0, default, 1f);
                    Main.dust[num503].velocity *= 2f;
                    Main.dust[num503].scale *= 1.15f;
                }
                Projectile.localAI[0] += 1f;
            }
            if (player.MinionDamage() != Projectile.Calamity().spawnedPlayerMinionDamageValue)
            {
                int damage2 = (int)((float)Projectile.Calamity().spawnedPlayerMinionProjectileDamageValue /
                    Projectile.Calamity().spawnedPlayerMinionDamageValue *
                    player.MinionDamage());
                Projectile.damage = damage2;
            }
            if (Math.Abs(Projectile.velocity.X) > 0.2f)
            {
                Projectile.spriteDirection = -Projectile.direction;
            }
            float lightScalar = (float)Main.rand.Next(90, 111) * 0.01f;
            lightScalar *= Main.essScale;
            Lighting.AddLight(Projectile.Center, 1.2f * lightScalar, 0.8f * lightScalar, 0f * lightScalar);
            bool correctMinion = Projectile.type == ModContent.ProjectileType<SonOfYharon>();
            player.AddBuff(ModContent.BuffType<YharonKindleBuff>(), 3600);
            if (correctMinion)
            {
                if (player.dead)
                {
                    modPlayer.aChicken = false;
                }
                if (modPlayer.aChicken)
                {
                    Projectile.timeLeft = 2;
                }
            }
            if (Projectile.ai[0] == 2f)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 3)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame > 2)
                {
                    Projectile.frame = 0;
                }
			}
			else
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter > 12)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				if (Projectile.frame > 3)
				{
					Projectile.frame = 0;
				}
			}

			Projectile.ChargingMinionAI(1000f, 2000f, 3000f, 500f, 1, 30f, 8f, 4f, new Vector2(0f, -60f), 40f, 8f, true, true);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
            int num214 = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = num214 * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, num214)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2((float)texture2D13.Width / 2f, (float)num214 / 2f), Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}
