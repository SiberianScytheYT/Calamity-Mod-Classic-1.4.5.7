using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Verdant : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Verdant");
/*
            Tooltip.SetDefault("Fires crystal leaves when enemies are near\n" +
			"A very agile yoyo");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 110;
            Item.knockBack = 6f;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<VerdantYoyo>();
            Item.shootSpeed = 16f;

            Item.rare = 10;
            Item.autoReuse = true;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.value = Item.buyPrice(platinum: 1, gold: 20);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
