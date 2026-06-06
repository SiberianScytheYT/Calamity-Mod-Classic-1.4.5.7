using CalRD.Events;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class BossRush : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terminus");
/*
            Tooltip.SetDefault("A ritualistic artifact, thought to have brought upon The End many millennia ago\n" +
                                "Sealed away in the abyss, far from those that would seek to misuse it\n" +
                                "Activates Boss Rush Mode, using it again will deactivate Boss Rush Mode\n" +
                                "During the Boss Rush, all wires and wired devices will be disabled");
*/
        }

        public override void SetDefaults()
        {
            Item.rare = 1;
            Item.width = 28;
            Item.height = 28;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item123;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            for (int doom = 0; doom < 200; doom++)
            {
                NPC n = Main.npc[doom];
                if (!n.active)
                    continue;

                // will also correctly despawn EoW because none of his segments are boss flagged
                bool shouldDespawn = n.boss || n.type == NPCID.EaterofWorldsHead || n.type == NPCID.EaterofWorldsBody || n.type == NPCID.EaterofWorldsTail;
                if (shouldDespawn)
                {
                    n.active = false;
                    n.netUpdate = true;
                }
            }
            if (!BossRushEvent.BossRushActive)
            {
                BossRushEvent.BossRushStage = 0;
                BossRushEvent.BossRushActive = true;
                string key = "Hmm? Ah, another contender. Very well, may the ritual commence!";
                Color messageColor = Color.LightCoral;
                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }
            else
            {
                BossRushEvent.BossRushStage = 0;
                BossRushEvent.BossRushActive = false;
            }

            CalamityNetcode.SyncWorld();
            if (Main.netMode == NetmodeID.Server)
            {
                var netMessage = Mod.GetPacket();
                netMessage.Write((byte)CalRDMessageType.BossRushStage);
                netMessage.Write(BossRushEvent.BossRushStage);
                netMessage.Send();
            }
            return true;
        }
    }
}
