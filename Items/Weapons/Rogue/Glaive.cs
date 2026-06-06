using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Glaive : RogueWeapon
    {
        public static int BaseDamage = 45;
        public static float Knockback = 3f;
        public static float Speed = 10f;
        public static float StealthSpeedMult = 1.8f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glaive");
/*
            Tooltip.SetDefault("Stacks up to 3\n"
                               +"Stealth strikes are super fast and pierce infinitely");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = BaseDamage;
            Item.crit = 4;
            Item.Calamity().rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 1;
            Item.height = 1;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = Knockback;
            Item.value = Item.buyPrice(0, 1, 40, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.maxStack = 3;

            Item.shootSpeed = Speed;
            Item.shoot = ModContent.ProjectileType<GlaiveProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai1 = 0f;
            if (player.Calamity().StealthStrikeAvailable())
            {
                velocity.X *= StealthSpeedMult;
                velocity.Y *= StealthSpeedMult;
                ai1 = 1f;
            }

            int p = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, ai1);
            if (player.Calamity().StealthStrikeAvailable())
                Main.projectile[p].Calamity().stealthStrike = true;
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < Item.stack;
        }

    }
}
