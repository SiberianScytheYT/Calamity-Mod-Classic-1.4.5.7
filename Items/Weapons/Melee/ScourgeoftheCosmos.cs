using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.Hybrid;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class ScourgeoftheCosmos : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scourge of the Cosmos");
/*
            Tooltip.SetDefault("Throws a bouncing cosmic scourge that emits tiny homing cosmic scourges on death and tile hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.damage = 1300;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.height = 20;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<ScourgeoftheCosmosProj>();
            Item.shootSpeed = 15f;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ScourgeoftheCorruptor);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
            recipe.AddIngredient(ModContent.ItemType<XerocPitchfork>(), 200);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
