using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
	public class Slurpfish : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Slurpfish");
		}

		public override void SetDefaults()
		{
			Item.questItem = true;
			Item.maxStack = 1;
			Item.width = 30;
			Item.height = 30;
			Item.uniqueStack = true;
			Item.rare = ItemRarityID.Quest;
		}

		public override bool IsQuestFish()
		{
			return true;
		}

		public override bool IsAnglerQuestAvailable()
		{
			return NPC.downedBoss3;
		}

		public override void AnglerQuestChat(ref string description, ref string catchLocation)
		{
			description = this.GetLocalizedValue("QuestDescription");
			catchLocation = CalamityUtils.GetTextValue("Misc.Fishing.CaughtInBrimstoneCrag");
		}
	}
}
