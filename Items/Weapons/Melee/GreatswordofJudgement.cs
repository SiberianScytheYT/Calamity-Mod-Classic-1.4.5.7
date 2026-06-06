using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class GreatswordofJudgement : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Greatsword of Judgement");
/*
            Tooltip.SetDefault("A pale white sword from a forgotten land\n" +
                               "You can hear faint yet comforting whispers emanating from the blade\n" +
                               "'No matter where you may be you are never alone\n" +
                               "I shall always be at your side, my lord'\n" +
                               "Fires a white orb that emits white rain on death for a time");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 78;
            Item.damage = 60;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 78;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<JudgementProj>();
            Item.shootSpeed = 10f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar, 7);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
