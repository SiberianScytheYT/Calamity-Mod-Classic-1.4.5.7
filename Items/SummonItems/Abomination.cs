using CalRD.Items.Materials;
using CalRD.NPCs.PlaguebringerGoliath;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class Abomination : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abombination");
/*
            Tooltip.SetDefault("Calls in the airborne jungle abomination\n" +
                "Summons the Plaguebringer Goliath");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 20;
            Item.rare = 8;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneJungle && !NPC.AnyNPCs(ModContent.NPCType<PlaguebringerGoliath>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<PlaguebringerGoliath>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<PlaguebringerGoliath>());

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 10);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar,5);
            recipe.AddIngredient(ItemID.Stinger, 2);
            recipe.AddIngredient(ItemID.Obsidian, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
