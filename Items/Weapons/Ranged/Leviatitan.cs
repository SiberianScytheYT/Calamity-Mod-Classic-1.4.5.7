using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
	public class Leviatitan : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Leviatitan");
/*
            Tooltip.SetDefault("Fires green and normal water blasts");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 82;
            Item.height = 28;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AquaBlast>();
            Item.shootSpeed = 18f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
            if (Main.rand.NextBool(3))
            {
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<AquaBlastToxic>(), (int)((double)damage * 1.5), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<AquaBlast>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
