using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Lacerator : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lacerator");
/*
            Tooltip.SetDefault("Enemies that are hit by the yoyo will have their life drained\n" +
			"A very agile yoyo\n" +
			"Someone thought this was a viable weapon against DoG at one point lol");
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
            Item.damage = 100;
            Item.knockBack = 7f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<LaceratorYoyo>();
            Item.shootSpeed = 16f;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.value = Item.buyPrice(platinum: 1, gold: 40);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
