using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class AureusMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astrum Aureus Mask");
            if (Main.netMode != NetmodeID.Server)
            {
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            }
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.rare = 1;
            Item.vanity = true;
        }
    }
}
