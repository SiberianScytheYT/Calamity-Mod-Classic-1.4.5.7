using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class LeadTomahawk : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lead Tomahawk");
/*
            Tooltip.SetDefault("The tomahawks have more damage for a short time when initially thrown\n" +
                               "Stealth strikes pierce infinitely");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 40;
            Item.damage = 7;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.height = 36;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 1, 0);
            Item.rare = 0;
            Item.shoot = ModContent.ProjectileType<LeadTomahawkProj>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
                Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(50);
            recipe.AddRecipeGroup(RecipeGroupID.Wood);
            recipe.AddIngredient(ItemID.LeadBar);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
