using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Tools.ClimateChange
{
    public class AridArtifact : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arid Artifact");
/*
            Tooltip.SetDefault("Summons a sandstorm\n" +
                               "The sandstorm will happen shortly after the item is used");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = 5;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item66;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return CalamityWorld.downedDesertScourge;
        }

        // this is extremely ugly and has to be fully qualified because we add an item called Sandstorm
        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (Terraria.GameContent.Events.Sandstorm.Happening)
                CalamityUtils.StopSandstorm();
            else
                CalamityUtils.StartSandstorm();
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe.AddRecipeGroup("AnyAdamantiteBar", 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
