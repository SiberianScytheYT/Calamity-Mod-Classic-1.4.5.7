using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Rogue
{
    public class LionfishProj : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/Lionfish";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lionfish");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.Calamity().rogue = true;
			Projectile.timeLeft = CalamityUtils.SecondsToFrames(20f);
        }

        public override void AI()
        {
            int num982 = 25;
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= num982;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= 45f)
                {
                    float num986 = 0.98f;
                    float num987 = 0.35f;
                    Projectile.ai[1] = 45f;
                    Projectile.velocity.X = Projectile.velocity.X * num986;
                    Projectile.velocity.Y = Projectile.velocity.Y + num987;
                    if (Projectile.velocity.X < 0f)
                    {
                        Projectile.spriteDirection = -1;
                        Projectile.rotation = (float)Math.Atan2((double)-(double)Projectile.velocity.Y, (double)-(double)Projectile.velocity.X);
                    }
                    else
                    {
                        Projectile.spriteDirection = 1;
                        Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
                    }
                }
				if (Projectile.Calamity().stealthStrike)
				{
					if (Projectile.timeLeft % 8 == 0 && Projectile.owner == Main.myPlayer)
					{
						Vector2 vector62 = Main.player[Projectile.owner].Center - Projectile.Center;
						Vector2 vector63 = vector62 * -1f;
						vector63.Normalize();
						vector63 *= (float)Main.rand.Next(45, 65) * 0.1f;
						vector63 = vector63.RotatedBy((Main.rand.NextDouble() - 0.5) * 1.5707963705062866, default);
						int spike = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector63.X, vector63.Y, ModContent.ProjectileType<UrchinSpikeFugu>(), (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.5f, Projectile.owner, -10f, 0f);
						Main.projectile[spike].Calamity().forceRogue = true;
					}
				}
            }
            //Sticky Behaviour
            Projectile.StickyProjAI(15);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.ModifyHitNPCSticky(6, false);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 72;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int num193 = 0; num193 < 3; num193++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 14, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
            }
            for (int num194 = 0; num194 < 30; num194++)
            {
                int num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 14, 0f, 0f, 0, new Color(0, 255, 255), 2.5f);
                Main.dust[num195].noGravity = true;
                Main.dust[num195].velocity *= 3f;
                num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 14, 0f, 0f, 100, new Color(0, 255, 255), 1.5f);
                Main.dust[num195].velocity *= 2f;
                Main.dust[num195].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type == NPCID.KingSlime || target.type == NPCID.WallofFlesh || target.type == NPCID.WallofFleshEye ||
                target.type == NPCID.SkeletronHead || target.type == NPCID.SkeletronHand)
            {
                target.buffImmune[BuffID.Venom] = false;
            }
            target.AddBuff(BuffID.Venom, 240);
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(BuffID.Venom, 240);
        }
        */
    }
}
