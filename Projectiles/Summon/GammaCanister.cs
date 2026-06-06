using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Summon
{
    public class GammaCanister : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gamma Canister");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 28;
            Projectile.friendly = Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            if (!Main.player[Projectile.owner].Calamity().GammaCanisters.Contains(Projectile.whoAmI))
            {
                Main.player[Projectile.owner].Calamity().GammaCanisters.Add(Projectile.whoAmI);
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] == 80f)
            {
                for (int i = 0; i < 36; i++)
                {
                    Vector2 velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(3f, 8f);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, (int)CalamityDusts.SulfurousSeaAcid);
                    dust.velocity = velocity;
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(1.3f, 1.7f);
                }
            }
            Projectile.velocity = Vector2.UnitY * (float)Math.Sin(Projectile.ai[1] / 40f * MathHelper.TwoPi) * 0.5f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            const float totalRotations = 3f;
            const float outwardPositionDeltaStart = 8f;
            float angle = MathHelper.TwoPi * Projectile.ai[1] / totalRotations / 80f;
            float outwardPositionDelta = outwardPositionDeltaStart;
            if (Projectile.ai[1] < 80f && Projectile.ai[1] > 55f)
            {
                outwardPositionDelta = MathHelper.Lerp(outwardPositionDeltaStart, 0f, (Projectile.ai[1] - 55f) / 35f);
            }
            if (Projectile.ai[1] >= 80f)
            {
                outwardPositionDelta = 0f;
            }
            for (int i = 0; i < 3; i++)
            {
                angle += MathHelper.TwoPi / 3f * i;
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value,
                                 Projectile.Center + angle.ToRotationVector2() * outwardPositionDelta + Vector2.UnitY * (float)Math.Cos(angle) * 3f - Main.screenPosition,
                                 null,
                                 Color.White,
                                 Projectile.rotation,
                                 Projectile.Size * 0.5f,
                                 Projectile.scale,
                                 SpriteEffects.None,
                                 0f);
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Main.player[Projectile.owner].Calamity().GammaCanisters.Clear();
            for (int i = 0; i < 12; i++)
            {
                int idx = Dust.NewDust(Projectile.position, 8, 8, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 0.75f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                Main.dust[idx].scale = 1.8f;
                idx = Dust.NewDust(Projectile.position, 8, 8, (int)CalamityDusts.SulfurousSeaAcid, 0, 0, 0, default, 0.75f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                Main.dust[idx].scale = 1.8f;
            }
        }
    }
}
