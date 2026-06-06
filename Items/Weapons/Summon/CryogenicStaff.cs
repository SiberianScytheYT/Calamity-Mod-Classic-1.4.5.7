using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class CryogenicStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryogenic Staff");
/*
            Tooltip.SetDefault("Summons an animated ice construct to protect you\n"
                               +"Fire rate and range increase the longer it targets an enemy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item78;
            Item.shoot = ModContent.ProjectileType<IceSentry>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//CalamityUtils.OnlyOneSentry(player, type);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
            return false;
        }
    }
}
