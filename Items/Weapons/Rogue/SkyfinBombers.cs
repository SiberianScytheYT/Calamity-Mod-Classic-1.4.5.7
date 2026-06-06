using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class SkyfinBombers : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Skyfin Bombers");
/*
            Tooltip.SetDefault("Fishy bombers inbound!\n" +
			"Launches a skyfin nuke that homes in on enemies below it\n" +
			"Stealth strikes throw three skyfin nukes that home in regardless of enemy position");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.damage = 46;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 35;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.height = 30;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<SkyfinNuke>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
				for (int i = -8; i <= 8; i += 8)
				{
					Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(i));
					int stealth = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, 0f, player.whoAmI, 0f, 0f);
					Main.projectile[stealth].Calamity().stealthStrike = true;
				}
                return false;
            }
            return true;
        }
    }
}
