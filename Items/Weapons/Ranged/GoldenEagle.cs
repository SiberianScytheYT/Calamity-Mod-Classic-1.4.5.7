using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class GoldenEagle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Golden Eagle");
/*
            Tooltip.SetDefault("Fires 5 bullets at once");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 36;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 46;
            Item.height = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + 5f * 0.05f;
            float SpeedY = velocity.Y + 5f * 0.05f;
            float SpeedX2 = velocity.X - 5f * 0.05f;
            float SpeedY2 = velocity.Y - 5f * 0.05f;
            float SpeedX3 = velocity.X + 0f * 0.05f;
            float SpeedY3 = velocity.Y + 0f * 0.05f;
            float SpeedX4 = velocity.X - 10f * 0.05f;
            float SpeedY4 = velocity.Y - 10f * 0.05f;
            float SpeedX5 = velocity.X + 10f * 0.05f;
            float SpeedY5 = velocity.Y + 10f * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX2, SpeedY2, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX3, SpeedY3, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX4, SpeedY4, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX5, SpeedY5, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }
}
