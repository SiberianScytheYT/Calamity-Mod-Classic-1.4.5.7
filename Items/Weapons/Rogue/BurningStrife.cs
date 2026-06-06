using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class BurningStrife : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Burning Strife");
/*
            Tooltip.SetDefault("Throws a shadowflame spiky ball that bursts into flames\n" + 
                               "Stealth Strikes make the ball linger and explode more violently\n" +
                               "'Definitely not pocket safe'");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 16;
            Item.height = 28;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.damage = 80;
            Item.crit = 8;
            Item.shootSpeed = 8f;
            Item.shoot = ModContent.ProjectileType<BurningStrifeProj>();

            Item.Calamity().rogue = true; 
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float speedMult = 1f;
            if (player.Calamity().StealthStrikeAvailable())
			{
				speedMult = 1.25f;
			}
            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X * speedMult, velocity.Y * speedMult), ModContent.ProjectileType<BurningStrifeProj>(), damage, Item.knockBack, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable())
            {
                Main.projectile[proj].Calamity().stealthStrike = true;
                Main.projectile[proj].penetrate = 5;            
            }
            return false;
        }
    }
}
