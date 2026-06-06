using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing
{
    public class GlimmeringGemfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glimmering Gemfish");
/*
            Tooltip.SetDefault("Right click to extract gems");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 34;
            Item.height = 30;
            Item.rare = 2;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            int gemMin = 1;
            int gemMax = 3;
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Amethyst, 0.5f, gemMin, gemMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Topaz, 0.4f, gemMin, gemMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Sapphire, 0.3f, gemMin, gemMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Emerald, 0.2f, gemMin, gemMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Ruby, 0.15f, gemMin, gemMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Diamond, 0.1f, gemMin, gemMax);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Amber, 0.25f, gemMin, gemMax);
            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
            if (thorium != null)
			{
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, thorium.Find<ModItem>("Pearl").Type, 0.25f, gemMin, gemMax);
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, thorium.Find<ModItem>("Opal").Type, 0.25f, gemMin, gemMax);
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, thorium.Find<ModItem>("Onyx").Type, 0.25f, gemMin, gemMax);
			}
        }
    }
}
