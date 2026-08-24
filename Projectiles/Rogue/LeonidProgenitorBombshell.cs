using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using CalRD.Projectiles.Ranged;
using CalRD.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
	public class LeonidProgenitorBombshell : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/LeonidProgenitor";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Leonid Bombshell");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.Calamity().rogue = true;
        }

		public override void AI()
		{
			int randomDust = Utils.SelectRandom(Main.rand, new int[]
			{
				ModContent.DustType<AstralOrange>(),
				ModContent.DustType<AstralBlue>()
			});
			int astral = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, randomDust, 0f, 0f, 100, CalamityUtils.ColorSwap(LeonidProgenitor.blueColor, LeonidProgenitor.purpleColor, 1f), 0.8f);
			Main.dust[astral].noGravity = true;
			Main.dust[astral].velocity *= 0f;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.97f;
                    if (Projectile.velocity.X > -0.01f && Projectile.velocity.X < 0.01f)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/LeonidProgenitorGlow").Value;
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
		}

        public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
			if (Main.myPlayer == Projectile.owner)
			{
                int flash = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Flash>(), Projectile.damage, 0f, Projectile.owner, 0f, 1f);
				Main.projectile[flash].Calamity().forceRogue = true;
				Main.projectile[flash].usesLocalNPCImmunity = true;
				Main.projectile[flash].localNPCHitCooldown = 10;

				Vector2 pos = new Vector2(Projectile.Center.X + Projectile.width * 0.5f + Main.rand.Next(-201, 201), Main.screenPosition.Y - 600f - Main.rand.Next(50));
				Vector2 velocity = (Projectile.Center - pos) / 40f;
				int dmg = Projectile.damage / 2;
				int comet = Projectile.NewProjectile(Entity.GetSource_FromThis(), pos, velocity, ModContent.ProjectileType<LeonidCometBig>(), dmg, Projectile.knockBack, Projectile.owner, 0f, 0.5f + (float)Main.rand.NextDouble() * 0.3f);
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
			if (!Projectile.Calamity().stealthStrike || Main.myPlayer != Projectile.owner)
				return;

			Vector2 spinningpoint = new Vector2(0f, 6f);
			float num1 = MathHelper.ToRadians(45f);
			int cometAmt = 5;
			float num3 = -(num1 * 2f) / (cometAmt - 1f);
			for (int projIndex = 0; projIndex < cometAmt; ++projIndex)
			{
				int index2 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, spinningpoint.RotatedBy((double)num1 + (double)num3 * (double)projIndex, new Vector2()), ModContent.ProjectileType<LeonidCometSmall>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, -1f);
				Projectile proj = Main.projectile[index2];
				for (int index3 = 0; index3 < Projectile.localNPCImmunity.Length; ++index3)
				{
					proj.localNPCImmunity[index3] = Projectile.localNPCImmunity[index3];
				}
			}
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
			if (!Projectile.Calamity().stealthStrike || Main.myPlayer != Projectile.owner)
				return;

			Vector2 spinningpoint = new Vector2(0f, 6f);
			float num1 = MathHelper.ToRadians(45f);
			int cometAmt = 5;
			float num3 = -(num1 * 2f) / (cometAmt - 1f);
			for (int projIndex = 0; projIndex < cometAmt; ++projIndex)
			{
				int index2 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, spinningpoint.RotatedBy((double)num1 + (double)num3 * (double)projIndex, new Vector2()), ModContent.ProjectileType<LeonidCometSmall>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, -1f);
				Projectile proj = Main.projectile[index2];
				for (int index3 = 0; index3 < Projectile.localNPCImmunity.Length; ++index3)
				{
					proj.localNPCImmunity[index3] = Projectile.localNPCImmunity[index3];
				}
			}
        }
    }
}
