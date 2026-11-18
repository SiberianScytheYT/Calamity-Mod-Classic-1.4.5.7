using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.SunkenSeaCatches
{
	public class Serpentuna : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Serpentuna");
		}

		public override void SetDefaults()
		{
			Item.questItem = true;
			Item.maxStack = 1;
			Item.width = 36;
			Item.height = 40;
			Item.uniqueStack = true;
			Item.rare = ItemRarityID.Quest;
		}

		public override bool IsQuestFish()
		{
			return true;
		}

		public override bool IsAnglerQuestAvailable()
		{
			return Main.hardMode;
		}

		public override void AnglerQuestChat(ref string description, ref string catchLocation)
		{
			description = this.GetLocalizedValue("QuestDescription");
			catchLocation = CalamityUtils.GetTextValue("Misc.Fishing.CaughtInSunkenSea");
		}
	}
}
