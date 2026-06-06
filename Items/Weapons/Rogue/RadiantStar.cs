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
    public class RadiantStar : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Radiant Star");
/*
            Tooltip.SetDefault("Throws daggers that explode and split after a while\n" +
                "Stealth strike splits more with a devastating explosion");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 52;
            Item.damage = 55; //33
            Item.crit += 8;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 48;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = 9;
            Item.shoot = ModContent.ProjectileType<RadiantStarKnife>();
            Item.shootSpeed = 20f;
            Item.Calamity().rogue = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Prismalline>());
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 15);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
