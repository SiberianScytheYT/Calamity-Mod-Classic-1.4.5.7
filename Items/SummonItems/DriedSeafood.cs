using CalRD.Items.Materials;
using CalRD.NPCs.DesertScourge;
using CalRD.World;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class DriedSeafood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Desert Medallion");
/*
            Tooltip.SetDefault("The desert sand stirs...\n" +
                "Summons the Desert Scourge");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 20;
            Item.rare = 2;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneDesert && !NPC.AnyNPCs(ModContent.NPCType<DesertScourgeHead>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<DesertScourgeHead>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<DesertScourgeHead>());

			if (CalamityWorld.revenge)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<DesertScourgeHeadSmall>());
				else
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<DesertScourgeHeadSmall>());

				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<DesertScourgeHeadSmall>());
				else
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<DesertScourgeHeadSmall>());
			}

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SandBlock, 15);
            recipe.AddIngredient(ItemID.AntlionMandible, 3);
            recipe.AddIngredient(ItemID.Cactus, 10);
            recipe.AddIngredient(ModContent.ItemType<StormlionMandible>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
