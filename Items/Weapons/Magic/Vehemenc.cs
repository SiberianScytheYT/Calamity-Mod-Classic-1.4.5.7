using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
	public class Vehemenc : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vehemence");
/*
            Tooltip.SetDefault("Casts an intense energy blast\n" +
                               "Does far more damage the more HP an enemy has left\n" +
                               "Max damage is capped at 1,000,000\n" +
                               "If an enemy has full HP it will inflict several long-lasting debuffs\n" +
                               "Revengeance drop");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 590;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.75f;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item73;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Vehemence>();
            Item.shootSpeed = 16f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(25, 25);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Vehemence>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            player.AddBuff(BuffID.ManaSickness, 600, true);
            return false;
        }
    }
}
