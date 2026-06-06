using CalRD.Tiles.DraedonStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Placeables.DraedonStructures
{
    public class LabHologramProjectorItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lab Hologram Projector");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Orange;
            Item.Calamity().customRarity = CalamityRarity.DraedonRust;
            Item.createTile = ModContent.TileType<LabHologramProjector>();
        }
    }
}
