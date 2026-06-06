using CalRD.Items.Materials;
using CalRD.NPCs.DevourerofGods;
using CalRD.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class CosmicWorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cosmic Worm");
/*
            Tooltip.SetDefault("Summons the Devourer of Gods\n" +
				"SENTINEL WARNING TOOLTIP LINE HERE\n" +
				"Not consumable");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

		public override void ModifyTooltips(List<TooltipLine> list)
		{
			foreach (TooltipLine line2 in list)
			{
				if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
				{
					bool sentinelsNotDefeated = !CalamityWorld.downedSentinel1 || !CalamityWorld.downedSentinel2 || !CalamityWorld.downedSentinel3;
					line2.Text = sentinelsNotDefeated ? "WARNING! Some sentinels have not been truly defeated yet and will spawn at full power during this fight!" : "";
				}
			}
		}

		public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<DevourerofGodsHead>()) && !NPC.AnyNPCs(ModContent.NPCType<DevourerofGodsHeadS>()) && CalamityWorld.DoGSecondStageCountdown <= 0 && CalamityWorld.downedBossAny;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            string key = "You are no god...but I shall feast upon your essence regardless!";
            Color messageColor = Color.Cyan;
            CalamityUtils.DisplayLocalizedText(key, messageColor);

            SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/DevourerSpawn"), new Vector2((int)player.position.X, (int)player.position.Y));
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<DevourerofGodsHead>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<DevourerofGodsHead>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 3);
            recipe.AddIngredient(ModContent.ItemType<TwistingNether>());
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
			recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.IronBar,30);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 20);
			recipe.AddIngredient(ItemID.SoulofLight, 20);
			recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 30);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
    }
}
