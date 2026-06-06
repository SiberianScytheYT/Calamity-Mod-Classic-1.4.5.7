using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class WulfrumHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wulfrum Hammer");
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 28;
            Item.height = 38;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.useTurn = true;
            Item.hammer = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.tileBoost += 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WulfrumShard>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
