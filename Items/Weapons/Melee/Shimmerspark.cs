using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Shimmerspark : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shimmerspark");
/*
            Tooltip.SetDefault("Fires stars when enemies are near\n" +
			"A very agile yoyo");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 36;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 36;
            Item.knockBack = 3.5f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<ShimmersparkYoyo>();
            Item.shootSpeed = 12f;

            Item.rare = 5;
            Item.value = Item.buyPrice(gold: 36);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
