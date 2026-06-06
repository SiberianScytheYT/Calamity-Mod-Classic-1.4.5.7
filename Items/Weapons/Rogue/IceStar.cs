using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class IceStar : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ice Star");
/*
            Tooltip.SetDefault("Throws homing ice stars\n" +
                "Stealth strikes pierce infinitely and spawn ice shards on hit\n" +
                "Ice Stars are too brittle to be recovered after being thrown");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 62;
            Item.damage = 45;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.crit = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 2.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 62;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 5, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<IceStarProjectile>();
            Item.shootSpeed = 14f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[proj].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
