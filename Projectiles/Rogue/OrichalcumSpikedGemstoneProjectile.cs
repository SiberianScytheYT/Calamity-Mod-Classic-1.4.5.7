using CalRD.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Rogue
{
    public class OrichalcumSpikedGemstoneProjectile : ModProjectile
    {
        public override string Texture => "CalRD/Items/Weapons/Rogue/OrichalcumSpikedGemstone";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gemstone");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.aiStyle = 2;
            Projectile.penetrate = 6;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.ThrowingKnife;
            Projectile.Calamity().rogue = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Vector2 velocity = Projectile.velocity;
            if (Projectile.velocity.Y != velocity.Y && (velocity.Y < -3f || velocity.Y > 3f))
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            }
            if (Projectile.velocity.X != velocity.X)
            {
                Projectile.velocity.X = velocity.X * -0.5f;
            }
            if (Projectile.velocity.Y != velocity.Y && velocity.Y > 1f)
            {
                Projectile.velocity.Y = velocity.Y * -0.5f;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Main.myPlayer != Projectile.owner)
				return;

			for (int i = 0; i < 2; i++)
			{
				int direction = Main.player[Projectile.owner].direction;
				float xStart = Main.screenPosition.X;
				if (direction < 0)
					xStart += Main.screenWidth;
				float yStart = Main.screenPosition.Y + Main.rand.Next(Main.screenHeight);
				Vector2 startPos = new Vector2(xStart, yStart);
				Vector2 pathToTravel = target.Center - startPos;
				pathToTravel.X += Main.rand.NextFloat(-50f, 50f) * 0.1f;
				pathToTravel.Y += Main.rand.NextFloat(-50f, 50f) * 0.1f;
				float speedMult = 24f / pathToTravel.Length();
				pathToTravel.X *= speedMult;
				pathToTravel.Y *= speedMult;
				int petal = Projectile.NewProjectile(Entity.GetSource_FromThis(), startPos, pathToTravel, ProjectileID.FlowerPetal, Projectile.damage, 0f, Projectile.owner, 0f, 0f);
				Main.projectile[petal].Calamity().forceRogue = true;
			}
		}

        //public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
        /*
        {
			if (Main.myPlayer != Projectile.owner)
				return;

			for (int i = 0; i < 2; i++)
			{
				int direction = Main.player[Projectile.owner].direction;
				float xStart = Main.screenPosition.X;
				if (direction < 0)
					xStart += Main.screenWidth;
				float yStart = Main.screenPosition.Y + Main.rand.Next(Main.screenHeight);
				Vector2 startPos = new Vector2(xStart, yStart);
				Vector2 pathToTravel = target.Center - startPos;
				pathToTravel.X += Main.rand.NextFloat(-50f, 50f) * 0.1f;
				pathToTravel.Y += Main.rand.NextFloat(-50f, 50f) * 0.1f;
				float speedMult = 24f / pathToTravel.Length();
				pathToTravel.X *= speedMult;
				pathToTravel.Y *= speedMult;
				int petal = Projectile.NewProjectile(Entity.GetSource_FromThis(), startPos, pathToTravel, ProjectileID.FlowerPetal, Projectile.damage, 0f, Projectile.owner, 0f, 0f);
				Main.projectile[petal].Calamity().forceRogue = true;
			}
		}
		*/

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(2))
            {
                Item.NewItem(Entity.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<OrichalcumSpikedGemstone>());
            }
        }
    }
}
