using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
//using TerrariaOverhaul;

namespace CalRD.Items.Weapons.Ranged
{
    public class Barinade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Barinade");
/*
            Tooltip.SetDefault("Shoots electric bolt arrows that explode");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 44;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BoltArrow>();
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Arrow;
        }

        /*public void OverhaulInit()
        {
            this.SetTag("bow");
        }*/

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<BoltArrow>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
