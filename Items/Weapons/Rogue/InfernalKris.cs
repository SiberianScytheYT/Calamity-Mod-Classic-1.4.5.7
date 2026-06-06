using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class InfernalKris : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Infernal Kris");
/*
            Tooltip.SetDefault("Throws a burning dagger that starts spinning after travelling a short distance, inflicting additional damage while spinning\n" +
                "Stealth strikes cause the dagger to be engulfed in flames, exploding on contact with walls and enemies");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 32;
            Item.damage = 24;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 5, 0);
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<InfernalKrisProjectile>();
            Item.shootSpeed = 15f;
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
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient(ItemID.Obsidian, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
