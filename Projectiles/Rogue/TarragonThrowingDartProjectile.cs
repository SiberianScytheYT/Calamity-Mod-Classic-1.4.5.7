using CalRD.Dusts;
using CalRD.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class TarragonThrowingDartProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/TarragonThrowingDart";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dart");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.aiStyle = 113;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.BoneJavelin;
            Projectile.Calamity().rogue = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 3;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, (int)CalamityDusts.SulfurousSeaAcid, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(2))
            {
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<TarragonThrowingDart>());
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Main.myPlayer == Projectile.owner)
			{
				if (Projectile.Calamity().stealthStrike)
				{
					float random = Main.rand.Next(30, 90);
					float spread = random * 0.0174f;
					double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
					double deltaAngle = spread / 8f;
					for (int i = 0; i < 4; i++)
					{
						double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
						int proj1 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f) * 2f, (float)(Math.Cos(offsetAngle) * 5f) * 2f, ModContent.ProjectileType<TarraThornRight>(), Projectile.damage / 4, Projectile.knockBack / 4, Projectile.owner, 0f, 0f);
						int proj2 = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f) * 2f, (float)(-Math.Cos(offsetAngle) * 5f) * 2f, ModContent.ProjectileType<TarraThornRight>(), Projectile.damage / 4, Projectile.knockBack / 4, Projectile.owner, 0f, 0f);
					}
				}
			}
        }
    }
}
