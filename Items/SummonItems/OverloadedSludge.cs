using CalRD.Items.Materials;
using CalRD.NPCs.SlimeGod;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class OverloadedSludge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Overloaded Sludge");
/*
            Tooltip.SetDefault("It looks corrupted\n" +
                "Summons the Slime God");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = 4;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<SlimeGodCore>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGod>()) &&
                !NPC.AnyNPCs(ModContent.NPCType<SlimeGodSplit>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRun>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRunSplit>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<SlimeGodCore>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<SlimeGodCore>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EbonianGel>(), 25);
            recipe.AddRecipeGroup("AnyEvilBlock", 25);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 25);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
