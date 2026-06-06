using Terraria.ModLoader;
using CalRD.NPCs.AcidRain;
using Terraria.ID;

namespace CalRD.Items.SummonItems
{
	public class BloodwormItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloodworm");
		}

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 20;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.bait = 4444;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.makeNPC = (short)ModContent.NPCType<BloodwormNormal>();
        }
    }
}
