using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class StellarKnife : RogueWeapon
    {
        int knifeCount = 15;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stellar Knife");
/*
            Tooltip.SetDefault("Throws knives that stop middair and then home into enemies\n" +
                               "Stealth strikes throw a volley of " + knifeCount + " knives in a spread\n" +
                               "Za Warudo");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.damage = 50;
            Item.crit += 4;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 9;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<StellarKnifeProj>();
            Item.shootSpeed = 10f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable() && player.ownedProjectileCounts[ModContent.ProjectileType<StellarKnifeProj>()] < 10)
            {
                int spread = 20;
                for (int i = 0; i < knifeCount; i++)
                {
                    velocity.X *= 0.9f;
                    Vector2 perturbedspeed = new Vector2(velocity.X, velocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(spread));
                    Projectile.NewProjectile(source, position, perturbedspeed, type, damage, Item.knockBack, player.whoAmI, 1f, i % 5 == 0 ? 1f : 0f);
                    spread -= Main.rand.Next(1, 3);
                }
                return false;
            }
            return true;
        }
    }
}
