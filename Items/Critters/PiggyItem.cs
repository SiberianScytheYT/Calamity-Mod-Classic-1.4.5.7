using CalRD.NPCs.NormalNPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Critters
{
    public class PiggyItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Piggy");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            //item.CloneDefaults(2004); //Lightning Bug item
            Item.width = 26;
            Item.height = 24;
            Item.makeNPC = (short)ModContent.NPCType<Piggy>();
            Item.rare = 1;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }
    }
}
