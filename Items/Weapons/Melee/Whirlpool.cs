using CalRD.Items.Placeables;
using CalRD.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Whirlpool : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Riptide");
/*
            Tooltip.SetDefault("Sprays a spiral of aqua streams in random directions\n" +
			"A very agile yoyo");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 44;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 18;
            Item.knockBack = 1f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<RiptideYoyo>();
            Item.shootSpeed = 18f;

            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 2);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 7);
            recipe.AddIngredient(ModContent.ItemType<Navystone>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
