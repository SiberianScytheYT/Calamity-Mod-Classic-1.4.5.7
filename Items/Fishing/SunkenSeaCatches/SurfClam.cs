using Terraria.ID;
using Terraria.ModLoader;
using CalRD.World;

namespace CalRD.Items.Fishing.SunkenSeaCatches
{
	public class SurfClam : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Surf Clam");
		}

		public override void SetDefaults()
		{
			Item.questItem = true;
			Item.maxStack = 1;
			Item.width = 20;
			Item.height = 20;
			Item.uniqueStack = true;
			Item.rare = ItemRarityID.Quest;
		}

		public override bool IsQuestFish()
		{
			return true;
		}

		public override bool IsAnglerQuestAvailable()
		{
			return CalamityWorld.downedDesertScourge;
		}

		public override void AnglerQuestChat(ref string description, ref string catchLocation)
		{
			description = this.GetLocalizedValue("QuestDescription");
			catchLocation = CalamityUtils.GetTextValue("Misc.Fishing.CaughtInSunkenSea");
		}
	}
}
