using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Aorta : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aorta");
/*
            Tooltip.SetDefault("Fires homing blood when enemies are near\n" +
				"An exceptionally agile yoyo");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 25;
            Item.knockBack = 4.25f;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<AortaYoyo>();
            Item.shootSpeed = 8f;

            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 4);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodSample>(), 6);
            recipe.AddIngredient(ItemID.Vertebrae, 3);
            recipe.AddIngredient(ItemID.CrimtaneBar, 3);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
