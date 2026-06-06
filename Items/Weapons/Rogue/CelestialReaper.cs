using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class CelestialReaper : RogueWeapon
    {
        public const int BaseDamage = 120;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Celestial Reaper");
/*
            Tooltip.SetDefault("Throws a fast homing scythe\n" +
                               "The scythe will bounce after hitting an enemy up to six times\n" +
                               "Stealth strikes create damaging afterimages");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = BaseDamage;
            Item.width = 66;
            Item.height = 76;
            Item.useAnimation = 31;
            Item.useTime = 31;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.rare = 10;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(platinum: 1); //sell price of 20 gold
            Item.shoot = ModContent.ProjectileType<CelestialReaperProjectile>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            float strikeValue = player.Calamity().StealthStrikeAvailable().ToInt(); //0 if false, 1 if true
            int p = Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<CelestialReaperProjectile>(), damage, Item.knockBack, player.whoAmI, strikeValue);
            if (player.Calamity().StealthStrikeAvailable())
                Main.projectile[p].Calamity().stealthStrike = true;
            return false;
        }
    }
}
