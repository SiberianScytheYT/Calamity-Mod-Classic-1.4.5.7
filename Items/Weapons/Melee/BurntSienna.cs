using CalRD.Projectiles.Healing;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class BurntSienna : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Burnt Sienna");
/*
            Tooltip.SetDefault("Causes enemies to erupt into healing projectiles on death");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 14;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = Item.useTime = 21;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.shootSpeed = 5f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0 && !player.moonLeech)
            {
                float randomSpeedX = (float)Main.rand.Next(3);
                float randomSpeedY = (float)Main.rand.Next(3, 5);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.statLife <= 0 && !player.moonLeech)
            {
                float randomSpeedX = (float)Main.rand.Next(3);
                float randomSpeedY = (float)Main.rand.Next(3, 5);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, -randomSpeedY, ModContent.ProjectileType<BurntSiennaProj>(), 0, 0f, player.whoAmI);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 246);
            }
        }
    }
}
