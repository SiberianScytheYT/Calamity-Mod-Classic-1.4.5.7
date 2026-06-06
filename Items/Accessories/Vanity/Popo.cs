using CalRD.CalPlayer;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories.Vanity
{
    public class Popo : ModItem
    {
        public override void Load()
        {
            // this code wasn't here originally but it's here out of necessity, what with having to load these assets without an implicit path to them
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/Vanity/Popo_Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/Vanity/PopoNoseless_Head", EquipType.Head, name: "PopoNoselessHead");
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/Vanity/Popo_Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/Vanity/Popo_Legs", EquipType.Legs, this);
            }
        }
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magic Scarf and Hat");
/*
            Tooltip.SetDefault("Don't let the demons steal your nose\n" +
				"Transforms the holder into a snowman");
*/
            if (Main.netMode == NetmodeID.Server)
                return;
            
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;

            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 30;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip1")
					{
						line2.Text = "Transforms the holder into a snowman\n" +
						"Provides heat and cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.snowman = true;
            if (hideVisual)
            {
                modPlayer.snowmanHide = true;
            }
        }
    }
}
