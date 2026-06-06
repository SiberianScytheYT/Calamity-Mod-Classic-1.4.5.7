using CalRD.NPCs.Providence;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class ProfanedCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Profaned Core");
/*
            Tooltip.SetDefault("The core of the unholy flame\n" +
                "Summons Providence\n" +
                "Should be used during daytime");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Providence>()) && (player.ZoneHallow || player.ZoneUnderworldHeight) && CalamityWorld.downedBossAny;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
			SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/ProvidenceSpawn"), new Vector2((int)player.position.X, (int)player.position.Y));
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int npc = NPC.NewNPC(Entity.GetSource_FromThis(), (int)(player.position.X + Main.rand.Next(-500, 501)), (int)(player.position.Y - 250f), ModContent.NPCType<Providence>(), 1);
				Main.npc[npc].timeLeft *= 20;
				CalamityUtils.BossAwakenMessage(npc);
			}
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Providence>());

			return true;
        }
    }
}
