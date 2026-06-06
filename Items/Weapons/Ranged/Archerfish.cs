using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Archerfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Archerfish");
/*
            Tooltip.SetDefault("Fires a stream of water");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 78;
            Item.height = 36;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ArcherfishShot>();
            Item.shootSpeed = 11f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, -5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ArcherfishShot>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
