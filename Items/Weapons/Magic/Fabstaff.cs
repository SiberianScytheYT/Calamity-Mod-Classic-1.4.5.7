using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class Fabstaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fabstaff");
/*
            Tooltip.SetDefault("Casts a bouncing beam that splits when enemies are near it");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 616;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 50;
            Item.width = 84;
            Item.height = 84;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FabRay>();
            Item.shootSpeed = 6f;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 100);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 50);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
