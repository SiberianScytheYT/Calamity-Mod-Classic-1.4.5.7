using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Yoyos;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Azathoth : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Azathoth");
/*
            Tooltip.SetDefault("Fires cosmic orbs that blast nearby enemies with lasers\n" +
			"A very agile yoyo\n" +
			"Destroy the universe in the blink of an eye");
*/
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 54;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = 200;
            Item.knockBack = 6f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<AzathothYoyo>();
            Item.shootSpeed = 16f;

            Item.rare = 10;
            Item.autoReuse = true;
            Item.Calamity().customRarity = CalamityRarity.Developer;
            Item.value = Item.buyPrice(platinum: 5);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Terrarian);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>(), 3);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
