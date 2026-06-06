using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class RougeSlash : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rouge Slash");
/*
            Tooltip.SetDefault("Fires a wave of 3 rouge air slashes");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 30;
            Item.width = 28;
            Item.height = 32;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item91;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<RougeSlashLarge>();
            Item.shootSpeed = 24f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<RougeSlashLarge>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 0.8f, velocity.Y * 0.8f, ModContent.ProjectileType<RougeSlashMedium>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 0.6f, velocity.Y * 0.6f, ModContent.ProjectileType<RougeSlashSmall>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }
}
