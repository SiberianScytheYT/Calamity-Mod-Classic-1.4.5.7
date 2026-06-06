using CalRD.Items.Materials;
using CalRD.NPCs.Calamitas;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class BlightedEyeball : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye of Desolation");
/*
            Tooltip.SetDefault("Tonight is going to be a horrific night...\n" +
                "Summons Calamitas\n" +
                "Not consumable");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.rare = 6;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Calamitas>()) && !NPC.AnyNPCs(ModContent.NPCType<CalamitasRun3>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Calamitas>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Calamitas>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 7);
            recipe.AddIngredient(ModContent.ItemType<BlightedLens>(), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
