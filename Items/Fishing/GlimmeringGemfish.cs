using Terraria;
using Terraria.GameContent.ItemDropRules;
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

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int gemMin = 1;
            int gemMax = 3;
            itemLoot.Add(ItemID.Amethyst, 2, gemMin, gemMax);
            itemLoot.Add(ItemID.Topaz, 2, gemMin, gemMax);
            itemLoot.Add(new CommonDrop(ItemID.Sapphire, 30, gemMin, gemMax, 100));
            itemLoot.Add(ItemID.Emerald, 5, gemMin, gemMax);
            itemLoot.Add(new CommonDrop(ItemID.Ruby, 15, gemMin, gemMax, 100));
            itemLoot.Add(ItemID.Diamond, 10, gemMin, gemMax);
            itemLoot.Add(ItemID.Amber, 4, gemMin, gemMax);
            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
            if (thorium is not null)
			{
                try
                {
                    itemLoot.Add(thorium.Find<ModItem>("Pearl").Type, 4, gemMin, gemMax); 
                    itemLoot.Add(thorium.Find<ModItem>("Opal").Type, 4, gemMin, gemMax);
                    itemLoot.Add(thorium.Find<ModItem>("Onyx").Type, 4, gemMin, gemMax);
                }
                catch
                {
                    CalRD.Instance.Logger.Debug("One of the items in this file got renamed internally. Please report this in the Calamity Mod Classic 1.4.5.7 bug reports thread in the #bug-reports forum found in the YuHther mods discord server.");
                }
            }
        }
    }
}
