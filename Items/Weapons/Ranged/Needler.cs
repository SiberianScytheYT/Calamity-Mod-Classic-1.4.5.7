using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Needler : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Needler");
/*
            Tooltip.SetDefault("Fires needles that stick to enemies and explode");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 44;
            Item.height = 26;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item108;
            Item.autoReuse = true;
            Item.shootSpeed = 9f;
            Item.shoot = ModContent.ProjectileType<NeedlerProj>();
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<NeedlerProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
