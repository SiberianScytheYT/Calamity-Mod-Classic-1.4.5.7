using CalRD.Buffs.StatDebuffs;
using CalRD.Dusts;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
	public class AcidicReed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reed");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
			Projectile.penetrate = 1;
        }

        public override void AI()
        {
			if (Projectile.ai[0] == 1f)
			{
                Terraria.Audio.SoundStyle saxSound = Utils.SelectRandom(Main.rand, new Terraria.Audio.SoundStyle[]
                {
					new SoundStyle("CalRD/Sounds/Item/Saxophone/Sax1"),
					new SoundStyle("CalRD/Sounds/Item/Saxophone/Sax2"),
					new SoundStyle("CalRD/Sounds/Item/Saxophone/Sax3"),
					new SoundStyle("CalRD/Sounds/Item/Saxophone/Sax4"),
					new SoundStyle("CalRD/Sounds/Item/Saxophone/Sax5"),
					new SoundStyle("CalRD/Sounds/Item/Saxophone/Sax6")
                });
				SoundEngine.PlaySound(saxSound, Projectile.position);
				Projectile.ai[0] = 0f;
			}
            if (Projectile.velocity.Y < 10f)
                Projectile.velocity.Y += 0.25f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
        }
        */

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 2; i++)
            {
                int idx = Dust.NewDust(Projectile.position, 8, 8, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 0.75f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                idx = Dust.NewDust(Projectile.position, 8, 8, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 0.75f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
            }
        }
    }
}
