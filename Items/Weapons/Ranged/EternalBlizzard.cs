using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class EternalBlizzard : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eternal Blizzard");
/*
            Tooltip.SetDefault("Fires an additional icicle arrow that shatters on impact");
*/
        }
        public override void SetDefaults()
        {
            Item.damage = 73;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 62;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IcicleArrowProj>();
            Item.shootSpeed = 11f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float SpeedX = velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
			float SpeedY = velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
			int index = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<IcicleArrowProj>(), (int)(damage * 0.7f), Item.knockBack, player.whoAmI, 0f, 0f);
			Main.projectile[index].noDropItem = true;

            return true;
        }
    }
}
