using CalRD.NPCs.NormalNPCs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items
{
	public class SuperDummy : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Super Dummy");
/*
			Tooltip.SetDefault("Creates a super dummy\n" +
				"Regenerates 1 million life per second\n" +
				"Will not die when taking damage over time from debuffs\n" +
				"Right click to kill all super dummies");
*/
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.width = 20;
			Item.height = 30;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.value = 0;
			Item.rare = 1;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			if (player.altFunctionUse == 2)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.type == ModContent.NPCType<SuperDummyNPC>() && npc.active)
					{
                        npc.active = false;
                        npc.netUpdate = true;
                        SoundEngine.PlaySound(SoundID.NPCDeath2, npc.Center);
					}
				}
			}
			else if (player.whoAmI == Main.myPlayer)
			{
				int x = (int)Main.MouseWorld.X - 9;
				int y = (int)Main.MouseWorld.Y - 20;

				// In single player, just spawn the dummy.
				if (Main.netMode == NetmodeID.SinglePlayer)
					NPC.NewNPC(new EntitySource_ItemUse(player, Item), x, y, ModContent.NPCType<SuperDummyNPC>());

				// Otherwise, send a message to the server indicating that a Super Dummy should be spawned at this position.
				else
				{
					var netMessage = Mod.GetPacket();
					netMessage.Write((byte)CalRDMessageType.SpawnSuperDummy);
					netMessage.Write(x);
					netMessage.Write(y);
					netMessage.Send();
				}
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TargetDummy);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
