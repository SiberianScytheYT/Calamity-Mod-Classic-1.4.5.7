using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class ProfanedTrident : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Infernal Spear");
/*
            Tooltip.SetDefault("Throws a homing spear that explodes on enemy hits\n" +
			"Stealth strikes summon fireballs as they fly before exploding into a fiery fountain");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 72;
            Item.damage = 1000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<InfernalSpearProjectile>();
            Item.shootSpeed = 28f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }
    }
}
