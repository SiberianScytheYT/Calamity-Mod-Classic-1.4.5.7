using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Rogue
{
    public class TotalityBreakers : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Totality Breakers");
/*
            Tooltip.SetDefault("Explodes into highly flammable black tar\n"
                               +"Tar oils enemies and sets them alight\n"
                               +"Stealth strikes leak tar as they fly");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 30;
            Item.damage = 55;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 28;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<TotalityFlask>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MolotovCocktail, 50);
            recipe.AddIngredient(ModContent.ItemType<SolarVeil>(), 10);
            recipe.AddIngredient(ModContent.ItemType<ConsecratedWater>());
            recipe.AddIngredient(ModContent.ItemType<DesecratedWater>());
            recipe.AddIngredient(ModContent.ItemType<SpentFuelContainer>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
