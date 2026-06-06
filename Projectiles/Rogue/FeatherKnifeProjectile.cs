using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class FeatherKnifeProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/FeatherKnife";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Knife");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.aiStyle = 2; 
            Projectile.timeLeft = 600;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.Calamity().rogue = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void AI()
        {
            if (Projectile.timeLeft % 35 == 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X / 20, 2), ModContent.ProjectileType<StickyFeatherAero>(), (int)(Projectile.damage * 0.4), Projectile.knockBack, Projectile.owner);
					Main.projectile[proj].Calamity().forceRogue = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int proj = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X / 20, 2), ModContent.ProjectileType<StickyFeatherAero>(), (int)(Projectile.damage * 0.69), Projectile.knockBack, Projectile.owner);
			Main.projectile[proj].Calamity().forceRogue = true;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(2) && !Projectile.Calamity().stealthStrike)
            {
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<FeatherKnife>());
            }
        }
    }
}
