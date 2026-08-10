using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee.Yoyos
{
    public class MicrowaveYoyo : ModProjectile
    {
        private const float Radius = 100f;
        private SlotId mmmmmm;
		private bool spawnedAura = false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Microwave");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 450f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 14f;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 99;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            // Sound is done manually, so that it can loop correctly.
            ActiveSound MMMMMMMMMMMMMMM;

            bool mmmIsThere = SoundEngine.TryGetActiveSound(mmmmmm, out MMMMMMMMMMMMMMM);

            if (!mmmIsThere)
            {
                mmmmmm = SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/MMMMMMMMMMMMM") {IsLooped = true}, Projectile.Center);
            }

            else if (mmmIsThere)
            {
                if (MMMMMMMMMMMMMMM.IsPlaying)
                    MMMMMMMMMMMMMMM.Position = Projectile.Center;

                else
                    MMMMMMMMMMMMMMM.Resume();
            }
            
            // Spawn invisible but damaging aura projectile
            if (Projectile.owner == Main.myPlayer && !spawnedAura)
            {
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MicrowaveAura>(), (int)(Projectile.damage * 0.35), Projectile.knockBack, Projectile.owner, Projectile.identity, 0f);
				spawnedAura = true;
            }

            // Dust circle appears for all players, even though the aura projectile is only spawned by the owner
            int numDust = (int)(0.2f * MathHelper.TwoPi * Radius);
            float angleIncrement = MathHelper.TwoPi / (float)numDust;
            Vector2 dustOffset = new Vector2(Radius, 0f);
            dustOffset = dustOffset.RotatedByRandom(MathHelper.TwoPi);
            for (int i = 0; i < numDust; i++)
            {
                dustOffset = dustOffset.RotatedBy(angleIncrement);
                int dustType = Utils.SelectRandom(Main.rand, new int[]
                {
                        ModContent.DustType<AstralOrange>(),
                        ModContent.DustType<AstralBlue>()
                });
                int dust = Dust.NewDust(Projectile.Center, 1, 1, dustType);
                Main.dust[dust].position = Projectile.Center + dustOffset;
                Main.dust[dust].fadeIn = 1f;
                Main.dust[dust].velocity *= 0.2f;
                Main.dust[dust].scale = 0.1599999999f;
            }

			if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
				Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            // no idea why this wasn't here
            SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/MicrowaveBeep"), Projectile.Center);
            ActiveSound MMMMMMMMMMMMMMM;
            if (SoundEngine.TryGetActiveSound(mmmmmm, out MMMMMMMMMMMMMMM))
            {
                MMMMMMMMMMMMMMM.Stop();
                //No more dispose function?
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Rectangle frame = new Rectangle(0, 0, 20, 16);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalRD/Projectiles/Melee/Yoyos/MicrowaveYoyoGlow").Value, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, Projectile.Size / 2, 1f, SpriteEffects.None, 0f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
        }
    }
}
