using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class BrackishFlask : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brackish Flask");
/*
            Tooltip.SetDefault("Explodes into poisonous seawater blasts\n" +
			"Stealth strikes summon a brackish spear spike");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.damage = 60;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 35;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.height = 30;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<BrackishFlaskProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
