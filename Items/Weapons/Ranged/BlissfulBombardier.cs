using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class BlissfulBombardier : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blissful Bombardier");
/*
            Tooltip.SetDefault("Fires flare rockets");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<Nuke>();
            Item.useAmmo = AmmoID.Rocket;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Nuke>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
