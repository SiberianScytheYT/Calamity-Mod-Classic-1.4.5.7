using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class StormfrontRazor : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stormfront Razor");
/*
            Tooltip.SetDefault("Throws a throwing knife that leaves sparks as it travels.\n" +
                               "Stealth strike causes the knife to be faster and leave a huge shower of sparks as it travels");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 5;

            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.damage = 50;
            Item.crit += 8;
            Item.knockBack = 7f;
            Item.shoot = ModContent.ProjectileType<StormfrontRazorProjectile>();
            Item.shootSpeed = 7f;
            Item.Calamity().rogue = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Cinquedea>());
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 4);
            recipe.AddIngredient(ItemID.Feather, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position, new Vector2(velocity.X * 1.6f, velocity.Y * 1.6f), ModContent.ProjectileType<StormfrontRazorProjectile>(), damage, Item.knockBack, player.whoAmI, 0, 40f);
                Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            else
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<StormfrontRazorProjectile>(), damage, Item.knockBack, player.whoAmI, 0, 1);
                return false;
            }
        }
    }
}
