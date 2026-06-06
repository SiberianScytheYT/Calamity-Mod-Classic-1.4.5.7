using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class EffluviumBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Effluvium Bow");
/*
            Tooltip.SetDefault("Fires two mist arrows at once");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 51;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 26;
            Item.height = 70;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MistArrow>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 2; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-30, 31) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-30, 31) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<MistArrow>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
