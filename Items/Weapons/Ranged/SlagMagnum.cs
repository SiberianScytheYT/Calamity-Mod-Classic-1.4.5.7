using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class SlagMagnum : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Slag Magnum");
/*
            Tooltip.SetDefault("Shoots fossil shards that split into additional shards on death");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 58;
            Item.height = 24;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shootSpeed = 22f;
            Item.shoot = ModContent.ProjectileType<SlagRound>();
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SlagRound>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
