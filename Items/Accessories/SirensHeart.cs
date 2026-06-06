using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class SirensHeart : ModItem
    {
        public override void Load()
        {
            // this code wasn't here originally but it's here out of necessity, what with having to load these assets without an implicit path to them
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/SirenTrans_Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/SirenTrans_Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/SirenTrans_Legs", EquipType.Legs, this);
            }
        }
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aquatic Heart");
/*
            Tooltip.SetDefault("Transforms the holder into a water elemental\n" +
                "Going underwater gives you a buff\n" +
                "Greatly reduces breath loss and provides a small amount of light in the abyss\n" +
                "Enemies become frozen when they touch you\n" +
                "You have a layer of ice around you that absorbs 20% damage but breaks after one hit\n" +
                "After 30 seconds the ice shield will regenerate\n" +
                "Wow, you can swim now!\n" +
                "Most of these effects are only active after Skeletron has been defeated\n" +
                "Revengeance drop");
*/
            if (Main.netMode != NetmodeID.Server)
            {
                int sirenHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
                int sirenBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
                int sirenLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
                ArmorIDs.Head.Sets.DrawHead[sirenHead] = false;
                ArmorIDs.Body.Sets.HidesTopSkin[sirenBody] = true;
                ArmorIDs.Body.Sets.HidesArms[sirenBody] = true;
                ArmorIDs.Legs.Sets.HidesBottomSkin[sirenLegs] = true; 
                //ArmorIDs.Shoe.Sets.OverridesLegs[sirenLegs] = true;
            }
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity4BuyPrice;
            Item.rare = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.sirenBoobs = true;
            if (hideVisual)
                modPlayer.sirenBoobsHide = true;
        }
    }
}
