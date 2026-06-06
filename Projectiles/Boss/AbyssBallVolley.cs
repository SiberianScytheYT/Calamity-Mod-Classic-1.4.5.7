using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Boss
{
    public class AbyssBallVolley : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyss Ball Volley");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
			Projectile.alpha = 60;
			Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
			if (Projectile.timeLeft < 60)
				Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);

			if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item33, Projectile.position);
            }

            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 173, 0f, 0f);
            }
        }

		public override bool CanHitPlayer(Player target) => Projectile.timeLeft >= 60;

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if (Projectile.timeLeft < 60)
				return;

			target.AddBuff(BuffID.Weak, 120);
        }
    }
}
