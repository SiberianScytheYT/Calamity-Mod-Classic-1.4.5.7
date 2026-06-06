using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class HeavenfallenStardisk : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heavenfallen Stardisk");
/*
            Tooltip.SetDefault("Throws a stardisk upwards which then launches itself towards your mouse cursor,\n" +
                               "explodes into several astral energy bolts if the thrower is moving vertically when throwing it and during its impact\n" +
							   "Stealth strikes rain astral energy bolts from the sky");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 38;
            Item.height = 34;
            Item.damage = 115;
            Item.crit += 20;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<HeavenfallenStardiskBoomerang>();
            Item.shootSpeed = 10f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int proj = Projectile.NewProjectile(source, position.X, position.Y, 0f, -10f, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[proj].Calamity().stealthStrike = true;
            }
			else
			{
				Projectile.NewProjectile(source, position.X, position.Y, 0f, -10f, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			}	
            return false;
        }
    }
}
