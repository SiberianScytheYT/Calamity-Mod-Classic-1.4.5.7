using CalRD.Events;
using CalRD.Items.Materials;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems.Invasion
{
    public class CausticTear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Caustic Tear");
/*
            Tooltip.SetDefault("Causes an acidic downpour in the Sulphurous Sea");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 99;
            Item.rare = 1;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !CalamityWorld.rainingAcid;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            CalamityNetcode.SyncWorld();
            AcidRainEvent.TryStartEvent(true);
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 4);
            recipe.AddCondition(Condition.NearWater);
            recipe.Register();
        }
    }
}
