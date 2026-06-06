using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Rogue
{
    public class TitaniumShuriken : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Titanium Shuriken");
/*
            Tooltip.SetDefault("Stealth strikes act like a boomerang that spawns clones on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 38;
            Item.damage = 37;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 9;
            Item.crit = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 9;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 38;
            Item.maxStack = 999;
            Item.value = 2000;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<TitaniumShurikenProjectile>();
            Item.shootSpeed = 16f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                Main.projectile[stealth].usesLocalNPCImmunity = true;
                Main.projectile[stealth].aiStyle = -1;
                Main.projectile[stealth].extraUpdates = 1;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(30);
            recipe.AddIngredient(ItemID.TitaniumBar);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
