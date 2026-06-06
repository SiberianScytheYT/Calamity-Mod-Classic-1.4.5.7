using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class DivineHatchet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seeking Scorcher");
/*
            Tooltip.SetDefault("May your enemies burn in hell for the sins they have committed\n" +
			"Throws a holy boomerang that seeks out up to three enemies before returning to the player");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.damage = 241;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 17;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.height = 62;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<DivineHatchetBoomerang>();
            Item.shootSpeed = 14f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PossessedHatchet);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 5);
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 9);
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
