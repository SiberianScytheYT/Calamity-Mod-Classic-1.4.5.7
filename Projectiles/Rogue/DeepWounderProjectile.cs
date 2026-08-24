using CalRD.Buffs.StatDebuffs;
using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Projectiles.Rogue
{
    public class DeepWounderProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/DeepWounder";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Deep Wounder Projectile");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;

            if (Projectile.Calamity().stealthStrike)
            {
                int spriteWidth = 52;
                int spriteHeight = 48;
                Vector2 spriteCenter = Projectile.Center - new Vector2(spriteWidth / 2, spriteHeight / 2);

                for (int i = 0; i < 30; i++)
                {
                    int dust = Dust.NewDust(spriteCenter, spriteWidth, spriteHeight, 33, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 1f);
                    Main.dust[dust].noGravity = true;
                }

                if (Main.rand.NextBool(5))
                {
                    Vector2 waterVelocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                    waterVelocity.Normalize();
                    waterVelocity *= 3;
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), spriteCenter, waterVelocity, ModContent.ProjectileType<DeepWounderWater>(), 20, 1, Projectile.owner, 0, 0);
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 120);
            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 120);
            if (Projectile.Calamity().stealthStrike)
            {
                target.AddBuff(ModContent.BuffType<CrushDepth>(), 120);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 120);
            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 120);
            if (Projectile.Calamity().stealthStrike)
            {
                target.AddBuff(ModContent.BuffType<CrushDepth>(), 120);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.direction > 0)
            {
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.FlipHorizontally, 0f);
            }
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            Projectile.Kill();
            return false;
        }
    }
}
