using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class FrostyFlare : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frosty Flare");
/*
            Tooltip.SetDefault("Do not insert in flare gun\n" +
				"Sticks to enemies\n" +
                "Generates a localized hailstorm\n" +
				"Stealth strikes trail snowflakes and summon phantom copies instead of ice shards");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 32;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.width = 10;
            Item.height = 22;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = false;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 0, 8, 0);
            Item.rare = 6;
            Item.shoot = ModContent.ProjectileType<FrostyFlareProj>();
            Item.shootSpeed = 22f;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.Calamity().StealthStrikeAvailable())
			{
				int gemstone = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<FrostyFlareStealth>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[gemstone].Calamity().stealthStrike = true;
				return false;
			}
			return true;
        }
    }
}
