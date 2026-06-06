using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class CrystalBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crystal Blade");
/*
            Tooltip.SetDefault("Fires slow-moving clouds of crystal dust");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.damage = 45;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<CrystalDust>();
            Item.shootSpeed = 3f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrystalShard, 15);
            recipe.AddIngredient(ItemID.CobaltSword);
            recipe.AddIngredient(ItemID.PixieDust, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrystalShard, 15);
            recipe.AddIngredient(ItemID.PalladiumSword);
            recipe.AddIngredient(ItemID.PixieDust, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
