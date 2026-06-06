using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class GreatswordofBlah : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Greatsword of Blah");
/*
            Tooltip.SetDefault("A pale white sword from a forgotten land\n" +
                               "You can hear faint yet comforting whispers emanating from the blade\n" +
                               "'No matter where you may be you are never alone\n" +
                               "I shall always be at your side, my lord'\n" +
                               "Fires a rainbow blade that emits rainbow rain on death for a time");
*/
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 102;
            Item.damage = 300;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<JudgementBlah>();
            Item.shootSpeed = 6f;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GreatswordofJudgement>());
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
