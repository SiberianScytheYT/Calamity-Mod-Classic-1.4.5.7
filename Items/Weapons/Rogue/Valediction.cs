using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Valediction : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Valediction");
/*
            Tooltip.SetDefault("Throws a homing reaper scythe\n" +
			"Stealth strikes spawn razorblade typhoons on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 80;
            Item.damage = 405;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 64;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<ValedictionBoomerang>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
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
