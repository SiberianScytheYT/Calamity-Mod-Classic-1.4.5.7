using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using CalRD.Events;

namespace CalRD.Projectiles.Boss
{
    public class HolyBurnOrb : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/StarProj";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Orb");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
			Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
        }

        public override void AI()
        {
			if (Projectile.ai[0] < 240f)
			{
				Projectile.ai[0] += 1f;

				if (Projectile.timeLeft < 160)
					Projectile.timeLeft = 160;
			}

			if (Projectile.velocity.Length() < 16f)
				Projectile.velocity *= 1.01f;

			int index = Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height);
			Player player = Main.player[index];
			if (player is null || player.Calamity().lol)
				return;

			float playerDist = Vector2.Distance(player.Center, Projectile.Center);
            if (playerDist < 50f && !player.dead && Projectile.position.X < player.position.X + player.width && Projectile.position.X + Projectile.width > player.position.X && Projectile.position.Y < player.Bottom.Y && Projectile.Bottom.Y > player.position.Y)
            {
                int dmgAmt = (int)Projectile.ai[1];
                player.HealEffect(dmgAmt, false);
                player.statLife += dmgAmt;
                if (player.statLife > player.statLifeMax2)
                {
                    player.statLife = player.statLifeMax2;
                }
                if (player.statLife < 0 || CalamityWorld.armageddon)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " burst into sinless ash."), 1000.0, 0, false);
                }
                NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, index, dmgAmt);
                Projectile.Kill();
            }
        }

		public override bool PreDraw(ref Color lightColor)
		{
			float lerpMult = CalamityUtils.GetLerpValue(15f, 30f, Projectile.timeLeft, clamped: true) * CalamityUtils.GetLerpValue(240f, 200f, Projectile.timeLeft, clamped: true) * (1f + 0.2f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 30f / 0.5f * (MathHelper.Pi * 2f) * 3f)) * 0.8f;

			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Color baseColor = Main.dayTime ? new Color(255, 200, 100, 255) : new Color(100, 200, 255, 255);
			baseColor *= 0.5f;
			baseColor.A = 0;
			Color colorA = baseColor;
			Color colorB = baseColor * 0.5f;
			colorA *= lerpMult;
			colorB *= lerpMult;
			Vector2 origin = texture.Size() / 2f;
			Vector2 scale = new Vector2(0.5f, 1.5f) * lerpMult;

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver2, origin, scale, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, drawPos, null, colorA, 0f, origin, scale, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver2, origin, scale * 0.6f, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, drawPos, null, colorB, 0f, origin, scale * 0.6f, spriteEffects, 0);

			Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver4, origin, scale * 0.6f, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver4 * 3f, origin, scale * 0.6f, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver4, origin, scale * 0.36f, spriteEffects, 0);
			Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver4 * 3f, origin, scale * 0.36f, spriteEffects, 0);

			return false;
		}

		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 50);
			int dustType = Main.dayTime ? (int)CalamityDusts.ProfanedFire : (int)CalamityDusts.Nightwither;
			for (int d = 0; d < 10; d++)
            {
                int holy = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
                Main.dust[holy].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[holy].scale = 0.5f;
                    Main.dust[holy].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int d = 0; d < 15; d++)
            {
                int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 3f);
                Main.dust[fire].noGravity = true;
                Main.dust[fire].velocity *= 5f;
                fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
                Main.dust[fire].velocity *= 2f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			int buffType = Main.dayTime ? ModContent.BuffType<HolyFlames>() : ModContent.BuffType<Nightwither>();
			target.AddBuff(buffType, 60);
		}
    }
}
