using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Rogue
{
    public class EpidemicShredder : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Epidemic Shredder");
/*
            Tooltip.SetDefault("Contrary to its name, it will probably cause an epidemic if used incorrectly\n" +
							   "Throws a plagued boomerang that releases plague seekers when it hits tiles or enemies\n" +
                               "Stealth strikes cause the boomerang release plague seekers constantly as it travels");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.damage = 52;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<EpidemicShredderProjectile>();
            Item.shootSpeed = 18f;
            Item.Calamity().rogue = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 velocity1 = new Vector2(velocity.X, velocity.Y);
            float strikeValue = player.Calamity().StealthStrikeAvailable().ToInt(); //0 if false, 1 if true
            int projectileIndex = Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<EpidemicShredderProjectile>(), damage, Item.knockBack, player.whoAmI, ai1: strikeValue);
            if (player.Calamity().StealthStrikeAvailable())
            {
                Main.projectile[projectileIndex].Calamity().stealthStrike = strikeValue == 1f;
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ChlorophyteBar, 20);
            recipe.AddIngredient(ItemID.Nanites, 150);
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
