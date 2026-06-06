using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
	public class DragoonDrizzlefish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragoon Drizzlefish");
/*
            Tooltip.SetDefault("Fires an inaccurate spread of fireballs\n"
                               +"The brimstone sac appears to contain fuel\n"
                               +"Revenge is a dish best served flaming hot");
*/
            Item.staff[Item.type] = true; //so it doesn't look weird af when holding it
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DrizzlefishFireball>();
            Item.shootSpeed = 11f;
        }

        public override Vector2? HoldoutOrigin() //so it looks normal when holding
        {
            return new Vector2(10, 10);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 rotated = new Vector2(velocity.X, velocity.Y);
			rotated = rotated.RotatedByRandom(MathHelper.ToRadians(10f));
			velocity.X = rotated.X;
			velocity.Y = rotated.Y;
			int shotType = ModContent.ProjectileType<DrizzlefishFireball>();
			if (Main.rand.NextBool(2))
			{
				shotType = ModContent.ProjectileType<DrizzlefishFire>();
			}
			else
			{
				shotType = ModContent.ProjectileType<DrizzlefishFireball>();
			}
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, shotType, damage, Item.knockBack, player.whoAmI, 0f, Main.rand.Next(2));
            return false;
        }
    }
}
