using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Weapons.Rogue;

namespace CalRD.Items.Fishing.AstralCatches
{
    public class GacruxianMollusk : RogueWeapon
    {
        public static int BaseDamage = 36;
        public static float Knockback = 5f;
        public static float Speed = 15f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gacruxian Mollusk");
/*
            Tooltip.SetDefault("Releases homing sparks while traveling\n" +
            "Stealth strikes release homing snails that create even more sparks");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = BaseDamage;
            Item.rare = 5;
            Item.knockBack = Knockback;
            Item.autoReuse = true;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 24;
            Item.height = 22;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<GacruxianProj>();
            Item.shootSpeed = Speed;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.Calamity().rogue = true;
            //item.maxStack = 999; not consumable because imagine knowing how to fish up more than one of an item
            //item.consumable = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<GacruxianProj>(), damage, Item.knockBack, player.whoAmI, 0f, 1f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
