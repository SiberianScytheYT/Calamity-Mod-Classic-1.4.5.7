using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Malevolence : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Malevolence");
/*
            Tooltip.SetDefault("Fires two plague arrows that explode into bees on death");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 51;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 58;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item97;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<PlagueArrow>();
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<PlagueArrow>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
