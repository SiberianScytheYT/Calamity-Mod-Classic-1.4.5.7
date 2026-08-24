using CalRD.Buffs.StatDebuffs;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class IcebreakerHammer : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/Icebreaker";

		private int explosionCount = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Icebreaker");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.Calamity().rogue = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 120;
            AIType = ProjectileID.WoodenBoomerang;
			Projectile.coldDamage = true;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Projectile.owner == Main.myPlayer)
			{
				target.AddBuff(BuffID.Frostburn, 180);

				if (Projectile.Calamity().stealthStrike)
				{
					if (explosionCount < 3) //max amount of explosions to prevent worm memes
					{
						int ice = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CosmicIceBurst>(), (int)(Projectile.damage * 1.5), Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
						Main.projectile[ice].Calamity().forceRogue = true;
						explosionCount++;
					}

					int buffType = ModContent.BuffType<GlacialState>();
					float radius = 112f; // 7 blocks

					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC nPC = Main.npc[i];
						if (nPC.active && !nPC.dontTakeDamage && !nPC.buffImmune[buffType] && Vector2.Distance(Projectile.Center, nPC.Center) <= radius)
						{
							if (nPC.FindBuffIndex(buffType) == -1)
								nPC.AddBuff(buffType, 180, false);
						}
					}
				}
			}
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if (Projectile.owner == Main.myPlayer)
			{
				target.AddBuff(BuffID.Frostburn, 180);

				if (Projectile.Calamity().stealthStrike)
				{
					//no explosion count cap in pvp
					int ice = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CosmicIceBurst>(), (int)(Projectile.damage * 1.5), Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
					Main.projectile[ice].Calamity().forceRogue = true;

					int buffType = ModContent.BuffType<GlacialState>();
					float radius = 112f; // 7 blocks

					for (int i = 0; i < Main.maxPlayers; i++)
					{
						Player owner = Main.player[Projectile.owner];
						Player player = Main.player[i];
						if ((owner.team != player.team || player.team == 0)  && player.hostile && owner.hostile && !player.dead && !player.buffImmune[buffType] && Vector2.Distance(Projectile.Center, player.Center) <= radius)
						{
							if (player.FindBuffIndex(buffType) == -1)
								player.AddBuff(buffType, 180, false);
						}
					}
				}
			}
        }
    }
}
