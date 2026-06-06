using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Cinquedea : RogueWeapon
    {
        public static int BaseDamage = 36;
        public static float Knockback = 5f;
        public static float Speed = 8f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cinquedea");
/*
            Tooltip.SetDefault("Stealth strikes home in after hitting an enemy");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = BaseDamage;
            Item.rare = 3;
            Item.knockBack = Knockback;
            Item.crit = 8;
            Item.autoReuse = true;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<CinquedeaProj>();
            Item.shootSpeed = Speed;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<CinquedeaProj>(), damage, Item.knockBack, player.whoAmI, 0f, 1f);
                Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
