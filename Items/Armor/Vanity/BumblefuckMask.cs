using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class BumblefuckMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dragonfolly Mask");
            if (Main.netMode != NetmodeID.Server)
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            }
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.rare = 1;
            Item.vanity = true;
        }
    }
}
