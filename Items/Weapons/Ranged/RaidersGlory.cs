using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class RaidersGlory : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Raider's Glory");
/*
            Tooltip.SetDefault("Fires ichor arrows with increased velocity\n" +
			"These arrows also cause enemies to drop more money");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.crit += 10;
            Item.width = 50;
            Item.height = 22;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projectile = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.IchorArrow, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[projectile].extraUpdates++;
            return false;
        }
    }
}
