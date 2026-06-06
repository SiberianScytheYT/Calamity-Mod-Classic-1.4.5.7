using CalRD.Items.Materials;
using CalRD.NPCs.CeaselessVoid;
using CalRD.NPCs.Signus;
using CalRD.NPCs.StormWeaver;
using CalRD.World;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
	public class RuneofCos : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Rune of Kos");
/*
            Tooltip.SetDefault("A relic of the profaned flame\n" +
                "Contains the power hunted relentlessly by the sentinels of the cosmic devourer\n" +
                "When used in certain areas of the world it will unleash them\n" +
                "Not consumable");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = 9;
            Item.UseSound = SoundID.Item44;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool CanUseItem(Player player)
        {
            return (player.ZoneSkyHeight || player.ZoneUnderworldHeight || player.ZoneDungeon) &&
                !NPC.AnyNPCs(ModContent.NPCType<StormWeaverHead>()) && !NPC.AnyNPCs(ModContent.NPCType<StormWeaverHeadNaked>()) && !NPC.AnyNPCs(ModContent.NPCType<CeaselessVoid>()) && !NPC.AnyNPCs(ModContent.NPCType<Signus>()) &&
				CalamityWorld.DoGSecondStageCountdown <= 0;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (player.ZoneDungeon)
            {
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<CeaselessVoid>());
					for (int num662 = 0; num662 < 2; num662++)
					{
						NPC.NewNPC(Entity.GetSource_FromThis(),(int)player.Center.X - 200, (int)player.Center.Y - 200, ModContent.NPCType<DarkEnergy>());
						NPC.NewNPC(Entity.GetSource_FromThis(),(int)player.Center.X + 200, (int)player.Center.Y - 200, ModContent.NPCType<DarkEnergy2>());
						NPC.NewNPC(Entity.GetSource_FromThis(),(int)player.Center.X, (int)player.Center.Y + 200, ModContent.NPCType<DarkEnergy3>());
					}
				}
				else
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<CeaselessVoid>());
			}
            else if (player.ZoneUnderworldHeight)
            {
				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Signus>());
				else
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Signus>());
			}
            else if (player.ZoneSkyHeight)
            {
				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<StormWeaverHead>());
				else
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<StormWeaverHead>());
			}

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 40);
			recipe.AddIngredient(ItemID.FragmentSolar, 5);
			recipe.AddIngredient(ItemID.LunarBar, 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
