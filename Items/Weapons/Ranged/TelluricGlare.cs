using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class TelluricGlare : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Telluric Glare");
/*
            Tooltip.SetDefault("Shoots an extremely fast energy arrow");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 54;
            Item.height = 92;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TelluricGlareProj>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<TelluricGlareProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
