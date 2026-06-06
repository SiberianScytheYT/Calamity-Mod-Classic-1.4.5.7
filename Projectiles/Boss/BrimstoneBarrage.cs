using CalRD.Buffs.DamageOverTime;
using CalRD.NPCs.SupremeCalamitas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
	public class BrimstoneBarrage : ModProjectile
    {
		public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Dart");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 690;
			CooldownSlot = 1;
        }

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.localAI[0]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = reader.ReadSingle();
		}

		public override void AI()
        {
			if (Projectile.velocity.Length() < (Projectile.ai[1] == 0f ? 14f : 10f))
				Projectile.velocity *= 1.01f;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

			if (Projectile.timeLeft < 60)
				Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);

			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;

				if (Projectile.ai[0] == 0f)
					Projectile.damage = Projectile.GetProjectileDamage(ModContent.NPCType<SupremeCalamitas>());
			}

			Lighting.AddLight(Projectile.Center, 0.75f, 0f, 0f);
        }

		public override bool CanHitPlayer(Player target) => Projectile.Opacity == 1f;

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if (Projectile.Opacity != 1f)
				return;

			if (Projectile.ai[0] == 0f)
			{
				target.AddBuff(ModContent.BuffType<AbyssalFlames>(), 180);
				target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
			}
			else
				target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
		}

        public override bool PreDraw(ref Color lightColor)
        {
			lightColor.R = (byte)(255 * Projectile.Opacity);
			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)	
        {
			target.Calamity().lastProjectileHit = Projectile;
		}
    }
}
