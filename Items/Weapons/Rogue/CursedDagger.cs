using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class CursedDagger : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cursed Dagger");
/*
            Tooltip.SetDefault("Throws bouncing daggers\n" +
			"Stealth strikes are showered in cursed fireballs");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 34;
            Item.damage = 34;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 34;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<CursedDaggerProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                Main.projectile[stealth].usesLocalNPCImmunity = true;
                return false;
            }
            return true;
        }
    }
}
