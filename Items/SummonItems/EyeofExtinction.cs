using CalRD.Items.Materials;
using CalRD.NPCs.SupremeCalamitas;
using CalRD.Tiles;
using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.World;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
	public class EyeofExtinction : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eye of Extinction");
/*
            Tooltip.SetDefault("Death\n" +
                "Summons Supreme Calamitas\n" +
                "Creates a large square arena of blocks around your player\n" +
                "Your player is the CENTER of the arena so be sure to use this item in a good location\n" +
                "During the battle, heart pickups will heal half as much HP\n" +
                "Not consumable");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitas>()) && CalamityWorld.downedBossAny;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            int surface = (int)Main.worldSurface;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < surface; j++)
                {
                    if (Main.tile[i, j] != null)
                    {
                        if (Main.tile[i, j].TileType == ModContent.TileType<ArenaTile>())
                        {
                            WorldGen.KillTile(i, j, false, false, false);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
                            }
                            else
                            {
                                WorldGen.SquareTileFrame(i, j, true);
                            }
                        }
                    }
                }
            }

            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<SupremeCalamitas>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<SupremeCalamitas>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<BlightedEyeball>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
