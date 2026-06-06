using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
namespace CalRD.Projectiles.DraedonsArsenal
{
    public class PhaseslayerBeam : ModProjectile
    {
        public const int Lifetime = 180;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Light Blade");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 120;
            Projectile.scale = 0.5f;
            Projectile.Size *= Projectile.scale;
            
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = Lifetime;
            Projectile.localNPCHitCooldown = 6;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            NPC potentialTarget = Projectile.Center.ClosestNPCAt(1000f);
            if (potentialTarget != null && Projectile.Distance(potentialTarget.Center) > 40f && Projectile.timeLeft > Lifetime - 60)
            {
                Projectile.velocity = (Projectile.velocity * 7f + Projectile.DirectionTo(potentialTarget.Center) * 24f) / 8f;
            }
            Projectile.frameCounter++;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.damage -= target.defense / 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D bladeTexture = ModContent.Request<Texture2D>(Texture).Value;
            MiscShaderData distortationShader = GameShaders.Misc["CalRD:LightDistortion"];

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Rectangle frame = bladeTexture.Frame(1, 5, 0, Projectile.frameCounter / 4 % 5);
            var drawData2 = new DrawData(bladeTexture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, bladeTexture.Size() / new Vector2(1f, 5f) * 0.5f, Projectile.scale * 1.2f, SpriteEffects.None, 0);
            distortationShader.Apply(drawData2);
            drawData2.Draw(Main.spriteBatch);

            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
