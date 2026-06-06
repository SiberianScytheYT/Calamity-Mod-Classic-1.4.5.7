using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class EidolonTablet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eidolon Tablet");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = 9;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCID.CultistBoss) && !NPC.LunarApocalypseIsUp && !NPC.AnyNPCs(NPCID.CultistTablet);
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC(Entity.GetSource_FromThis(),(int)player.Center.X + 30, (int)player.Center.Y - 90, NPCID.CultistBoss, 1);
                Main.npc[npc].direction = Main.npc[npc].spriteDirection = Math.Sign(player.Center.X - player.Center.X - 30f);
				Main.npc[npc].timeLeft *= 20;
				CalamityUtils.BossAwakenMessage(npc);
			}
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, NPCID.CultistBoss);

			return true;
        }
    }
}
