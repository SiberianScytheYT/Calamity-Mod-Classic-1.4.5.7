using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.CalPlayer;

namespace CalRD.Projectiles.Rogue
{
    public class PoisonBol : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/PoisonPack";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Poison Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 1200;
            Projectile.Calamity().rogue = true;
			Projectile.aiStyle = 14;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 45;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(20))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 44, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}

			if (Projectile.timeLeft < 20)
				Projectile.alpha += 13;
			if (Projectile.Calamity().stealthStrike == true)
			{
				Projectile.localAI[1]++;
				if ((double) Projectile.localAI[1] >= 35)
				{
					Vector2 velocity = Projectile.velocity;
					Vector2 vector2_1 = new Vector2((float) Main.rand.Next(-100, 101), (float) Main.rand.Next(-100, 101));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float) Main.rand.Next(10, 21) * 0.1f);
					if (Main.rand.Next(3) == 0)
						vector2_2 *= 2f;
					Vector2 vector2_3 = velocity * 0.25f + vector2_2;
					Vector2 vector2_5 = vector2_3 * 0.8f;
					int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X - vector2_5.X, Projectile.Center.Y - vector2_5.Y, vector2_5.X, vector2_5.Y, Main.rand.Next(569,572), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, 0f, 0f);
					Main.projectile[proj].Calamity().forceRogue = true;
					Main.projectile[proj].usesLocalNPCImmunity = true;
					Main.projectile[proj].localNPCHitCooldown = 45;
					Projectile.localAI[1] = 0.0f;
				}
            }

            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
			if (modPlayer.killSpikyBalls == true)
			{
				Projectile.active = false;
				Projectile.netUpdate = true;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Poisoned, 240);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*{
			target.AddBuff(BuffID.Poisoned, 240);
        }*/
    }
}

