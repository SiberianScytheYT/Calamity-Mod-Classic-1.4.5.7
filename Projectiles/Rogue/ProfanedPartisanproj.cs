using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class ProfanedPartisanproj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/ProfanedPartisan";

    	public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Profaned Partisan");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 3;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.Calamity().rogue = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 0.4f)
            {
                Projectile.ai[0] += 0.1f;
            }
            else
            {
                Projectile.tileCollide = true;
            }

            //Backwards projectile fix
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            //Rotating 45 degrees if shooting right
            if (Projectile.spriteDirection == 1)
            {
                Projectile.rotation += MathHelper.ToRadians(45f);
                DrawOffsetX = -26;
                DrawOriginOffsetX = 13;
                DrawOriginOffsetY = 2;
            }
            //Rotating 45 degrees if shooting right
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(45f);
                DrawOffsetX = 2;
                DrawOriginOffsetX = -13;
                DrawOriginOffsetY = 2;
            }

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.ProfanedFire, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.1f);
                Main.dust[d].position = Projectile.Center;
                Main.dust[d].velocity *= 0.3f;
                Main.dust[d].velocity += Projectile.velocity * 0.85f;
            }
            Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0.2f);

            if (Projectile.Calamity().stealthStrike) //Stealth strike
			{
				Vector2 spearPosition = new Vector2(Projectile.Center.X + Main.rand.NextFloat(-15f, 15f), Projectile.Center.Y + Main.rand.NextFloat(-15f, 15f));
				Vector2 spearSpeed = Projectile.velocity;
				if (Projectile.timeLeft % 18 == 0)
				{
					Projectile.NewProjectile(Entity.GetSource_FromThis(), spearPosition, spearSpeed, ModContent.ProjectileType<ProfanedPartisanspear>(), Projectile.damage / 10, Projectile.knockBack * 0.1f, Projectile.owner);
				}
			}
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.ProfanedFire, 0f, 0f, 50, default(Color), 2.6f);
            }
            SoundEngine.PlaySound(SoundID.Item45, Projectile.position);
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PartisanExplosion>(), Projectile.damage / 2, Projectile.knockBack * 0.5f, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        }
    }
}
