using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class Mourningstar : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mourningstar");
/*
            Tooltip.SetDefault("Launches two solar whip swords that explode on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.damage = 230;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2.5f;
            Item.UseSound = SoundID.Item116;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<MourningstarFlail>();
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 6);
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 6);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 6);//Quantities may or may not be intentional hue
            recipe.AddIngredient(ItemID.SolarEruption);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai3 = (Main.rand.NextFloat() - 0.75f) * 0.7853982f; //0.5
            float ai3X = (Main.rand.NextFloat() - 0.25f) * 0.7853982f; //0.5
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, ai3);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, ai3X);
            return false;
        }
    }
}
