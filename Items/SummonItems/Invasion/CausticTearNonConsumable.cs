using CalRD.Events;
using CalRD.Items.Materials;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems.Invasion
{
    public class CausticTearNonConsumable : ModItem
    {
        public override string Texture => "CalRD/Items/SummonItems/Invasion/CausticTear";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Caustic Tear");
/*
            Tooltip.SetDefault("Toggles the acid rain in the Sulphurous Sea");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 1;
            Item.rare = 6;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (!CalamityWorld.rainingAcid)
            {
                AcidRainEvent.TryStartEvent(true);
            }
            else
            {
                CalamityWorld.acidRainPoints = 0;
                CalamityWorld.triedToSummonOldDuke = false;
                AcidRainEvent.UpdateInvasion(false);
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CorrodedFossil>(), 10);
            recipe.AddIngredient(ModContent.ItemType<CausticTear>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
