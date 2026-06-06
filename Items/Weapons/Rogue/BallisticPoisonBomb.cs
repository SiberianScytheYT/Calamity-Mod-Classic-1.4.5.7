using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class BallisticPoisonBomb : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ballistic Poison Bomb");
/*
            Tooltip.SetDefault("Throws a sticky bomb that explodes into spikes and poison clouds\n" +
			"Stealth strikes throw three at once");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 30;
            Item.damage = 50;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 26;
            Item.knockBack = 6.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 38;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<BallisticPoisonBombProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int spread = 5;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 perturbedspeed = new Vector2(velocity.X + Main.rand.Next(-3,4), velocity.Y + Main.rand.Next(-3,4)).RotatedBy(MathHelper.ToRadians(spread));
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, perturbedspeed.X, perturbedspeed.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[proj].Calamity().stealthStrike = true;
                    spread -= Main.rand.Next(2,6);
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeafoamBomb>());
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 10);
            recipe.AddIngredient(ModContent.ItemType<SulphurousSand>(), 20);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
