using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Shellshooter : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shellshooter");
/*
            Tooltip.SetDefault("Shoots slow, powerful shells");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 38;
            Item.crit += 15;
            Item.useTime = 70;
            Item.useAnimation = 70;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Shell>();
            Item.shootSpeed = 1.5f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Shell>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }
}
