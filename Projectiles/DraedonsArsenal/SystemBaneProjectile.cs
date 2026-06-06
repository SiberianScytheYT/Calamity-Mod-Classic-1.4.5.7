using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRD.Projectiles.DraedonsArsenal
{
    public class SystemBaneProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/DraedonsArsenal/SystemBane";

        public SlotId ShittyMicrowaveMemeSound;
        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public const int LightningFireRate = 60;
        public const int FieldLightningFireRate = 45;
        public const float FieldRadius = 360f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("System Bane");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 480;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
			Projectile.StickToTiles(false, true);

            Time++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.velocity.Y < 15f)
            {
                Projectile.velocity.Y += 0.5f;
            }
            // Generate idle sparks.
            if (Time % 15f == 0f)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 229);
                dust.velocity = Main.rand.NextVector2Circular(10f, 10f);
                dust.fadeIn = 1.05f;
                dust.noGravity = true;
            }
            // Every so often, generate some lightning at a nearby enemy, if one exists.
            if (Time % LightningFireRate == 0f && Main.myPlayer == Projectile.owner)
            {
                NPC potentialTarget = Projectile.Center.ClosestNPCAt(900f);
                if (potentialTarget != null)
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(potentialTarget.Center) * 15f, ModContent.ProjectileType<SystemBaneLightning>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

            // Sometimes generate lightning from the outside of the energy field if the projectile was spawned by a stealth strike.
            if (Projectile.Calamity().stealthStrike)
            {
                NPC potentialTarget = Projectile.Center.ClosestNPCAt(FieldRadius);
                if (Time % FieldLightningFireRate == 0f && potentialTarget != null && Main.myPlayer == Projectile.owner)
                {
                    Vector2 spawnPosition = Projectile.Center + Main.rand.NextVector2CircularEdge(FieldRadius, FieldRadius);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), spawnPosition, potentialTarget.DirectionFrom(spawnPosition) * 14f, ModContent.ProjectileType<SystemBaneLightning>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }

            PlayMicrowaveSounds();
        }

        public void PlayMicrowaveSounds()
        {
            ActiveSound MMMMMMMMMMMMMMM;

            bool mmmIsThere = SoundEngine.TryGetActiveSound(ShittyMicrowaveMemeSound, out MMMMMMMMMMMMMMM);

            if (!mmmIsThere  && Projectile.Calamity().stealthStrike)
            {
                ShittyMicrowaveMemeSound = SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/MMMMMMMMMMMMM"), Projectile.Center);
            }

            else if (mmmIsThere)
            {
                if (MMMMMMMMMMMMMMM.IsPlaying)
                    MMMMMMMMMMMMMMM.Position = Projectile.Center;

                else
                    MMMMMMMMMMMMMMM.Resume();
            }
        }

        public override void OnKill(int timeLeft)
        {
            ActiveSound MMMMMMMMMMMMMMM;
            if (SoundEngine.TryGetActiveSound(ShittyMicrowaveMemeSound, out MMMMMMMMMMMMMMM))
            {
                MMMMMMMMMMMMMMM.Stop();
                //No more dispose function?
            }
        }

        public override void PostDraw(Color lightColor)
        {
            if (!Projectile.Calamity().stealthStrike)
                return;
            int totalCirclePoints = 55;
            float generalOpacity = Utils.GetLerpValue(0f, 30f, Projectile.timeLeft, true) * Utils.GetLerpValue(480f, 450f, Projectile.timeLeft, true);
            Texture2D lightningTexture = ModContent.Request<Texture2D>("CalRD/Projectiles/DraedonsArsenal/SystemBaneLightning").Value;
            for (int i = 0; i < totalCirclePoints; i++)
            {
                float angle = MathHelper.TwoPi * i / totalCirclePoints;
                float nextAngle = angle + MathHelper.TwoPi / totalCirclePoints;
                float radiusOffset = (float)Math.Cos(Main.GlobalTimeWrappedHourly * 65f);
                Vector2 start = Projectile.Center + angle.ToRotationVector2() * (FieldRadius + radiusOffset) - Main.screenPosition;
                Vector2 end = Projectile.Center + nextAngle.ToRotationVector2() * (FieldRadius + radiusOffset) - Main.screenPosition;

                DelegateMethods.f_1 = SystemBaneLightning.InnerLightningOpacity * generalOpacity;
                DelegateMethods.c_1 = SystemBaneLightning.InnerLightningColor;
                Utils.DrawLaser(Main.spriteBatch, lightningTexture, start, end, new Vector2(SystemBaneLightning.InnerLightningScale), new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));

                DelegateMethods.f_1 = SystemBaneLightning.OuterLightningOpacity * generalOpacity;
                DelegateMethods.c_1 = SystemBaneLightning.OuterLightningColor;
                Utils.DrawLaser(Main.spriteBatch, lightningTexture, start, end, new Vector2(SystemBaneLightning.OuterLightningScale), new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X *= 0.8f;
            return false;
        }
    }
}

