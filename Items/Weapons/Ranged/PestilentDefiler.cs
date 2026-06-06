using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class PestilentDefiler : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pestilent Defiler");
/*
            Tooltip.SetDefault("Fires a plague round that explodes and splits on death");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 135;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 20;
            Item.useTime = 37;
            Item.useAnimation = 37;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 9.5f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<SicknessRound>();
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SicknessRound>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
