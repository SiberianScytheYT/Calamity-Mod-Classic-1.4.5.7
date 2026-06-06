using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Ammo
{
    public class GodSlayerSlug : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("God Slayer Slug");
/*
            Tooltip.SetDefault("These bullets aren't finished.");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(silver: 4);
            Item.rare = ItemRarityID.Red;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
            Item.shoot = ModContent.ProjectileType<GodSlayerSlugMain>();
            Item.shootSpeed = 6f;
            Item.ammo = ItemID.MusketBall;
        }

        public override void AddRecipes()
        {
            /*
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>());
            recipe.AddRecipeGroup("NForEE");
            recipe.AddIngredient(ItemID.EmptyBullet, 999);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this, 999);
            recipe.AddRecipe();
			*/
        }
    }
}
