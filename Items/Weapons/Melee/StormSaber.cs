using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class StormSaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Storm Saber");
/*
            Tooltip.SetDefault("Fires two storm beams\n" +
			"One from blade and one from the sky");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.damage = 68;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 23;
            Item.useTime = 23;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 68;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<StormBeam>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)(damage * 0.8), Item.knockBack, player.whoAmI, 0f, 0f);

            Vector2 spawnPos = new Vector2(player.MountedCenter.X + Main.rand.Next(-200, 201), player.MountedCenter.Y - 600f);
            Vector2 targetPos = Main.MouseWorld + new Vector2(Main.rand.Next(-30, 31), Main.rand.Next(-30, 31));
            Vector2 velocity1 = targetPos - spawnPos;
            velocity1.Normalize();
            velocity1 *= 13f;

            Projectile.NewProjectile(source, spawnPos, velocity1, type, (int)(damage * 0.6), Item.knockBack, player.whoAmI);
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int num250 = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 187, (float)(player.direction * 2), 0f, 150, default, 1.3f);
                Main.dust[num250].velocity *= 0.2f;
            }
        }
    }
}
