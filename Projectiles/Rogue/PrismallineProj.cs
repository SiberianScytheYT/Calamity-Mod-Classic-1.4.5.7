using CalRD.Buffs.StatDebuffs;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class PrismallineProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/Prismalline";

        public bool hitEnemy = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Prismalline");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.aiStyle = 113;
            Projectile.timeLeft = 180;
            AIType = ProjectileID.BoneJavelin;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 40f)
            {
                int numProj = 4;
                int numSpecProj = 0;
                float rotation = MathHelper.ToRadians(50);
                if (Projectile.owner == Main.myPlayer)
                {
					if (!Projectile.Calamity().stealthStrike)
					{
						for (int i = 0; i < numProj + 1; i++)
						{
							Vector2 velocity = CalamityUtils.RandomVelocity(50f, 30f, 60f, 0.2f);
							if (numSpecProj < 2 && !hitEnemy)
							{
								Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Prismalline3>(), (int)(Projectile.damage * 1.1), Projectile.knockBack, Projectile.owner, 0f, 0f);
								++numSpecProj;
							}
							else
							{
								Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Prismalline2>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, 0f, 0f);
							}
						}
					}
					else //stealth strike
					{
						int shardCount = Main.rand.Next(2,5);
						for (int num252 = 0; num252 < shardCount; num252++)
						{
							Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
							int shard = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<AquashardSplit>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, 0f);
							Main.projectile[shard].Calamity().forceRogue = true;
							Main.projectile[shard].usesLocalNPCImmunity = true;
							Main.projectile[shard].localNPCHitCooldown = 10;
						}
						for (int i = 0; i < numProj + 1; i++)
						{
							Vector2 velocity = CalamityUtils.RandomVelocity(50f, 30f, 60f, 0.2f);
							Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Prismalline3>(), (int)(Projectile.damage * 1.15), Projectile.knockBack, Projectile.owner, 1f, 0f);
						}
					}
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 154, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
			if (Projectile.Calamity().stealthStrike)
			{
				int shardCount = Main.rand.Next(1,4);
				for (int s = 0; s < shardCount; s++)
				{
					Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
					int shard = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<AquashardSplit>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, 0f);
					Main.projectile[shard].Calamity().forceRogue = true;
					Main.projectile[shard].penetrate = 1;
				}
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitEnemy = true;
			if (Projectile.Calamity().stealthStrike)
				target.AddBuff(ModContent.BuffType<Eutrophication>(), 15);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            hitEnemy = true;
			if (Projectile.Calamity().stealthStrike)
				target.AddBuff(ModContent.BuffType<Eutrophication>(), 15);
        }
    }
}
