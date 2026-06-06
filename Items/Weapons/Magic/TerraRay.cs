using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class TerraRay : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Ray");
/*
            Tooltip.SetDefault("Casts an energy ray that splits if enemies are near it");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TerraBeam>();
            Item.shootSpeed = 6f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<NightsRay>());
            recipe.AddIngredient(ModContent.ItemType<ValkyrieRay>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CarnageRay>());
            recipe.AddIngredient(ModContent.ItemType<ValkyrieRay>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
