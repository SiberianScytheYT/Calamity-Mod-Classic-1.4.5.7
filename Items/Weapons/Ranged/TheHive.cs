using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class TheHive : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Hive");
/*
            Tooltip.SetDefault("Launches a variety of rockets that explode into bees on death");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 62;
            Item.height = 30;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BeeRPG>();
            Item.shootSpeed = 13f;
            Item.useAmmo = AmmoID.Rocket;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    type = ModContent.ProjectileType<GoliathRocket>();
                    break;
                case 1:
                    type = ModContent.ProjectileType<HiveMissile>();
                    break;
                case 2:
                    type = ModContent.ProjectileType<HiveBomb>();
                    break;
                case 3:
                    type = ModContent.ProjectileType<BeeRPG>();
                    break;
                default:
                    break;
            }
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
