using CalRD.CalPlayer;
using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
    public class AuricTeslaBodyArmor : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Auric Tesla Body Armor");
/*
            Tooltip.SetDefault("+100 max life\n" +
                       "25% increased movement speed\n" +
                       "Attacks have a 2% chance to do no damage to you\n" +
                       "8% increased damage and 5% increased critical strike chance\n" +
                       "You will freeze enemies near you when you are struck");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(1, 44, 0, 0);
            Item.defense = 48;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }
        
        public override void Load()
		{
			if (Main.netMode != NetmodeID.Server)
				EquipLoader.AddEquipTexture(Mod, Texture + "_Back", EquipType.Back, this);
		}
                
	    public override void EquipFrameEffects(Player player, EquipType type)
	    { 
		    if (player.body == Item.bodySlot)
			    player.back = (sbyte)EquipLoader.GetEquipSlot(Mod, Name, EquipType.Back);
	    }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip4")
					{
						line2.Text = "You will freeze enemies near you when you are struck\n" +
						"Provides heat and cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.fBarrier = true;
            modPlayer.godSlayerReflect = true;
            player.statLifeMax2 += 100;
            player.moveSpeed += 0.25f;
            player.GetDamage(DamageClass.Generic) += 0.08f;
            modPlayer.AllCritBoost(5);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SilvaArmor>());
            recipe.AddIngredient(ModContent.ItemType<GodSlayerChestplate>());
            recipe.AddIngredient(ModContent.ItemType<BloodflareBodyArmor>());
            recipe.AddIngredient(ModContent.ItemType<TarragonBreastplate>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 18);
			recipe.AddIngredient(ModContent.ItemType<FrostBarrier>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
