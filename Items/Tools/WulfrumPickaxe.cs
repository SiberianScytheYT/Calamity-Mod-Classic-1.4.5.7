using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class WulfrumPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Pickaxe");
        }

        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 46;
            Item.height = 38;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useTurn = true;
            Item.pick = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.tileBoost += 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WulfrumShard>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
