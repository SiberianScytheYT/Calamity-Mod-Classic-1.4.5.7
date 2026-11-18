using Terraria.ID;
using Terraria.ModLoader;
using CalRD.World;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
	public class Brimlish : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Brimlish");
		}

		public override void SetDefaults()
		{
			Item.questItem = true;
			Item.maxStack = 1;
			Item.width = 36;
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
			return CalamityWorld.downedBrimstoneElemental;
		}

		public override void AnglerQuestChat(ref string description, ref string catchLocation)
		{
			description = this.GetLocalizedValue("QuestDescription");
			catchLocation = CalamityUtils.GetTextValue("Misc.Fishing.CaughtInBrimstoneCrag");
		}
	}
}
