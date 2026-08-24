using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class ScarletDevilProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/ScarletDevil";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spear the Gungnir");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 116;
            Projectile.height = 116;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            CalamityPlayer modPlayer = Main.player[Projectile.owner].Calamity();
            Lighting.AddLight(Projectile.Center, 0.55f, 0.25f, 0f);
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 0.785f;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 130, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, new Color(255, 255, 255), 0.85f);
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] == 1f && modPlayer.StealthStrikeAvailable())
            {
                Projectile.Calamity().stealthStrike = true;
            }
            if ((Projectile.ai[0] %= 5f) == 0f)
            {
                int numProj = 2;
                float rotation = MathHelper.ToRadians(15);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < numProj; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(-Projectile.velocity.X / 3, -Projectile.velocity.Y / 3).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                        for (int j = 0; j < 2; j++)
                        {
                            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<ScarletDevilBullet>(), (int)((double)Projectile.damage * 0.03), 0f, Projectile.owner, 0f, 0f);
                            perturbedSpeed *= 1.05f;
                        }
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 150;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ScarletBlast>(), (int)((double)Projectile.damage * 0.0075), 0f, Projectile.owner, 0f, 0f);
            if (target.type == NPCID.TargetDummy || !Projectile.Calamity().stealthStrike || Main.player[Projectile.owner].moonLeech)
            {
                return;
            }
            Main.player[Projectile.owner].statLife += 120;
            Main.player[Projectile.owner].HealEffect(120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 150;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ScarletBlast>(), (int)((double)Projectile.damage * 0.0075), 0f, Projectile.owner, 0f, 0f);
            if (!Projectile.Calamity().stealthStrike)
            {
                return;
            }
            Main.player[Projectile.owner].statLife += 120;
            Main.player[Projectile.owner].HealEffect(120);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, new Color(100, 100, 100), ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return true;
        }
    }
}
