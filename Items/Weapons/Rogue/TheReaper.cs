using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class TheReaper : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Reaper");
/*
            Tooltip.SetDefault("Slice 'n dice\n" +
			"Stealth strikes throw four at once");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 80;
            Item.damage = 150;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 64;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<ReaperProjectile>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int spread = 10;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X + Main.rand.Next(-3,4), velocity.Y + Main.rand.Next(-3,4)).RotatedBy(MathHelper.ToRadians(spread));
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, perturbedspeed.X, perturbedspeed.Y, type, damage / 2, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[proj].Calamity().stealthStrike = true;
                    Main.projectile[proj].penetrate = 6;
                    spread -= Main.rand.Next(5,8);
                }
                return false;
            }
            return true;
        }
    }
}
