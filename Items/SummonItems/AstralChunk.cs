using CalRD.Items.Materials;
using CalRD.NPCs.AstrumAureus;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class AstralChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Chunk");
/*
            Tooltip.SetDefault("Summons Astrum Aureus");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = 7;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && player.Calamity().ZoneAstral && !NPC.AnyNPCs(ModContent.NPCType<AstrumAureus>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC(Entity.GetSource_FromThis(), (int)(player.position.X + Main.rand.Next(-50, 50)), (int)(player.position.Y - 150f), ModContent.NPCType<AstrumAureus>(), 1);
				Main.npc[npc].timeLeft *= 20;
				CalamityUtils.BossAwakenMessage(npc);
            }
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<AstrumAureus>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Stardust>(), 15);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
