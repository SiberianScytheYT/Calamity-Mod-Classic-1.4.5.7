using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class RealmRavager : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Realm Ravager");
/*
            Tooltip.SetDefault("Shoots a burst of explosive bullets");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.height = 32;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item38;
            Item.autoReuse = true;
            Item.shootSpeed = 30f;
            Item.shoot = ModContent.ProjectileType<RealmRavagerBullet>();
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numBullets = Main.rand.NextBool() ? 4 : 3;
            for (int index = 0; index < numBullets; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-75, 76) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-75, 76) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<RealmRavagerBullet>(), damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
            return false;
        }
    }
}
