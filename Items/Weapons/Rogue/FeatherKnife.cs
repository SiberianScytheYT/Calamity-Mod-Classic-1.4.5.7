using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class FeatherKnife : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Feather Knife");
/*
            Tooltip.SetDefault("Throws a knife which summons homing feathers\n"
                               +"Stealth strike throws a volley of knives");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 18;
            Item.damage = 25;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 32;
            Item.maxStack = 999;
            Item.value = 300;
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<FeatherKnifeProjectile>();
            Item.shootSpeed = 25f;
            Item.Calamity().rogue = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(30);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>());
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int spread = 6;
                for (int i = 0; i < 5; i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X + Main.rand.Next(-3, 4), velocity.Y + Main.rand.Next(-3, 4)).RotatedBy(MathHelper.ToRadians(spread));
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, perturbedspeed.X, perturbedspeed.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[proj].Calamity().stealthStrike = true;
                    spread -= Main.rand.Next(2, 6);
                }
                return false;
            }
            return true;
        }
    }
}
