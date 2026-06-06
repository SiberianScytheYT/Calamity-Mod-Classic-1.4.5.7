using CalRD.CalPlayer;
using CalRD.Projectiles.Rogue;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class FrostcrushValari : RogueWeapon
    {
        public static float Speed = 15f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Frostcrush Valari");
/*
            Tooltip.SetDefault("Fires a long ranged boomerang that explodes into icicles on hit\n"
                               +"Stealth strikes throw three short ranged boomerangs along with a spread of icicles");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 100;
            Item.knockBack = 12;
            Item.DamageType = DamageClass.Throwing;
            Item.crit = 16;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.width = 32;
            Item.height = 46;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = Speed;
            Item.shoot = ModContent.ProjectileType<ValariBoomerang>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer modPlayer = Main.player[Main.myPlayer].Calamity();
            //If stealth is full, shoot a spread of 3 boomerangs with reduced range and 6 to 10 icicles
            if (modPlayer.StealthStrikeAvailable())
            {
                int spread = 10;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(spread));
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, perturbedspeed.X, perturbedspeed.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
                    Main.projectile[proj].Calamity().stealthStrike = true;
                    spread -= 10;
                }
                int spread2 = 3;
                for (int i = 0; i < Main.rand.Next(6,11); i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X + Main.rand.Next(-3,4), velocity.Y + Main.rand.Next(-3,4)).RotatedBy(MathHelper.ToRadians(spread2));
                    Projectile.NewProjectile(source, position.X, position.Y, perturbedspeed.X, perturbedspeed.Y, (Main.rand.NextBool(2) ? ModContent.ProjectileType<Valaricicle>() : ModContent.ProjectileType<Valaricicle2>()), damage / 2, 0f, player.whoAmI, 0f, 0f);
                    spread2 -= Main.rand.Next(1,4);
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Kylie>());
            recipe.AddIngredient(ModContent.ItemType<CryoBar>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Voidstone>(), 40);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
