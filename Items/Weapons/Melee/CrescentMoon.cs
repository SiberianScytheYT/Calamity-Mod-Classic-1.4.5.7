using CalRD.Projectiles.Melee;
using CalRD.Items.Materials;
using CalRD.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class CrescentMoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crescent Moon");
/*
            Tooltip.SetDefault("People wanted the moon, let's bring the moon to them.\n" +
			"Fires a whip sword that summons homing crescent moons");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.damage = 500;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item82;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<CrescentMoonFlail>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai3 = (Main.rand.NextFloat() - 0.5f) * 0.7853982f; //0.5
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, ai3);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 8);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 3);
            recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 16);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
