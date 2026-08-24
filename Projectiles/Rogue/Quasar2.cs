using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class Quasar2 : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/Quasar";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Quasar");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            float num472 = Projectile.Center.X;
            float num473 = Projectile.Center.Y;
            float num474 = 600f;
            for (int num475 = 0; num475 < Main.maxNPCs; num475++)
            {
                if (Main.npc[num475].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[num475].Center, 1, 1) && !CalamityPlayer.areThereAnyDamnBosses)
                {
                    float npcCenterX = Main.npc[num475].position.X + (float)(Main.npc[num475].width / 2);
                    float npcCenterY = Main.npc[num475].position.Y + (float)(Main.npc[num475].height / 2);
                    float num478 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - npcCenterX) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - npcCenterY);
                    if (num478 < num474)
                    {
                        if (Main.npc[num475].position.X < num472)
                        {
                            Main.npc[num475].velocity.X += 0.25f;
                        }
                        else
                        {
                            Main.npc[num475].velocity.X -= 0.25f;
                        }
                        if (Main.npc[num475].position.Y < num473)
                        {
                            Main.npc[num475].velocity.Y += 0.25f;
                        }
                        else
                        {
                            Main.npc[num475].velocity.Y -= 0.25f;
                        }
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralBlue>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
			if (Projectile.Calamity().stealthStrike && Projectile.owner == Main.myPlayer)
			{
				int boomer = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadiantExplosion>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0f, 1f);
				Main.projectile[boomer].Calamity().stealthStrike = Projectile.Calamity().stealthStrike;
				Main.projectile[boomer].height = Main.projectile[boomer].width = 280;
				Main.projectile[boomer].Center = Projectile.Center;
			}
        }
    }
}
