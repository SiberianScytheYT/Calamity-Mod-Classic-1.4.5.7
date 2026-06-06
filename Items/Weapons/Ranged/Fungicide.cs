using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Fungicide : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fungicide");
/*
            Tooltip.SetDefault("Shoots fungal rounds that split on death");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 26;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FungiOrb>();
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FungiOrb>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
