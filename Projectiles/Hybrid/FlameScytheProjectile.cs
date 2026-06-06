using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Hybrid
{
    public class FlameScytheProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/FlameScythe";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scythe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 3;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;
            Projectile.alpha = 55;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.25f, 0.15f, 0f);
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, Main.rand.NextBool(3) ? 16 : 127, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Vector2 goreVec = new Vector2(Projectile.position.X + (float)(Projectile.width / 2) + Projectile.velocity.X, Projectile.position.Y + (float)(Projectile.height / 2) + Projectile.velocity.Y);
			if (Main.rand.NextBool(8))
			{
				int smoke = Gore.NewGore(Entity.GetSource_FromThis(), goreVec, default, Main.rand.Next(375, 378), 0.75f);
				Main.gore[smoke].behindTiles = true;
			}
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 6;
            target.AddBuff(BuffID.OnFire, 300);
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
				if (Projectile.CountsAsClass(DamageClass.Melee))
					Main.projectile[proj].Calamity().forceMelee = true;
				else
					Main.projectile[proj].Calamity().forceRogue = true;
            }
        }

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
            target.AddBuff(BuffID.OnFire, 300);
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
				if (Projectile.CountsAsClass(DamageClass.Melee))
					Main.projectile[proj].Calamity().forceMelee = true;
				else
					Main.projectile[proj].Calamity().forceRogue = true;
            }
        }
        */
    }
}
