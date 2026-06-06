using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class SpearofPaleolith : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Spear of Paleolith");
/*
            Tooltip.SetDefault("Throws an ancient spear that shatters enemy armor\n" +
                "Spears rain fossil shards as they travel\n" +
                "Stealth Strikes travel slower but further, raining more fossil shards");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 54;
            Item.damage = 65;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 27;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<SpearofPaleolithProj>();
            Item.shootSpeed = 35f;
            Item.Calamity().rogue = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 2);
            recipe.AddRecipeGroup("AnyAdamantiteBar", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int stabDevice = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stabDevice].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
