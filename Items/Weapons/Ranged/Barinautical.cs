using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Barinautical : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Barinautical");
/*
            Tooltip.SetDefault("Shoots a string of electric bolt arrows that explode");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 42;
            Item.useTime = 4;
            Item.reuseDelay = 20;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BoltArrow>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<BoltArrow>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
