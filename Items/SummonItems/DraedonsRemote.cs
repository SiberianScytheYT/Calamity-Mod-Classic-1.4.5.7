using CalRD.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class DraedonsRemote : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Draedon's Remote");
/*
            Tooltip.SetDefault("Mayhem...");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 20;
            Item.rare = 8;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && !NPC.AnyNPCs(NPCID.TheDestroyer) && !NPC.AnyNPCs(NPCID.SkeletronPrime) && !NPC.AnyNPCs(NPCID.Spazmatism) && !NPC.AnyNPCs(NPCID.Retinazer);
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            CalamityGlobalNPC.DraedonMayhem = true;
            CalamityNetcode.SyncWorld();
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.SpawnOnPlayer(player.whoAmI, NPCID.TheDestroyer);
				NPC.SpawnOnPlayer(player.whoAmI, NPCID.SkeletronPrime);
				NPC.SpawnOnPlayer(player.whoAmI, NPCID.Spazmatism);
				NPC.SpawnOnPlayer(player.whoAmI, NPCID.Retinazer);
			}
			else
			{
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, NPCID.TheDestroyer);
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, NPCID.SkeletronPrime);
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, NPCID.Spazmatism);
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, NPCID.Retinazer);
			}

			return true;
        }
    }
}
