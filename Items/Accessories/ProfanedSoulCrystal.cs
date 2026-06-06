using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.World;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    //Developer item, dedicatee: Mishiro Usui/Amber Sienna
    public class ProfanedSoulCrystal : ModItem
    {

        /**
         * Notes: Drops from providence if the only damage source during the fight is from typeless damage or the profaned soul and the owners of those babs do not have profaned crystal.
         * All projectiles are in ProfanedSoulCrystalProjectiles.cs in the summon projectile directory
         * The projectiles being created/fired on click happens in CalamityGlobalItem (there's a region specially for it so ctrl + f is your friend)
         * the day/night buffs are in calamityplayermisceffects
         * the bab projectiles are the same, just refactored ai to be more adhering to DRY principle
         * bab spears being fired happens at the bottom of calplayer
         * Animation of legs is postupdate, animation of wings is frameeffects.
         * Projectiles transformed are ONLY affected by alldamage and summon damage bonuses, likewise the weapon's base damage/usetime is NOT taken into account.
         * You enrage below or at 50% hp.
         */
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/ProfanedSoulTransHead", EquipType.Head, this);
            EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/ProfanedSoulTransBody", EquipType.Body, this);
            EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/ProfanedSoulTransLegs", EquipType.Legs, this);
            EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/Wings/ProfanedSoulTransWings", EquipType.Wings, this);
        }
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Profaned Soul Crystal");
/*
            Tooltip.SetDefault("Transforms you into an emissary of the profaned goddess\n" +
                "Requires 10 minion slots to use in order to grant the following effects\n" +
                "All non-summon weapons are converted into powerful summon variations\n" +
                "Falling below 50% life will empower these attacks\n" +
                "[c/f05a5a:Transforms Melee attacks into a barrage of spears]\n" +
                "[c/3a83e4:Transforms Magic attacks into a powerful splitting fireball]\n" +
                "[c/85e092:Transforms Ranged attacks into a flurry of fireballs and meteors]\n" +
                "[c/e97451:Transforms Rogue attacks into a deadly crystalline spiral]\n" +
                "Summons and empowers the profaned babs to fight alongside you\n" +
                "You are no longer affected by burn out when hit\n" +
                "Provides buffs depending on the time of day\n" +
                "Provides heat and cold protection in Death Mode\n" +
				"Thinking back, it was a boring life\n" +
				"[c/FFBF49:And so we burn it all in the name of purity]");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;

            if (Main.netMode == NetmodeID.Server)
                return;

            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;

            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
            ArmorIDs.Legs.Sets.OverridesLegs[equipSlotLegs] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.accessory = true;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */
        {
            return !player.Calamity().pArtifact;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!CalamityWorld.downedSCal)
            {
                int index = 0;
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name.StartsWith("Tooltip"))
                    {
                        if (line.Name == "Tooltip0")
                        {
                            index = tooltips.IndexOf(line);
                        } 
                        else
                        {
                            line.Text = "";
                        }
                    }
                    else if (line.Mod == "Terraria" && line.Text.Contains("Sell price"))
                    {
                        line.Text = "";
                    }
                        
                }
                
                tooltips.Insert(index+1, new TooltipLine(CalRD.Instance, "Tooltip1", "[c/f05a5a:The soul within this crystal has been defiled by the powerful magic of a supreme witch]\nMerchants will reject a defiled soul such as this."));
            }
            else if (Main.player[Main.myPlayer].Calamity().profanedCrystalBuffs)
            {
                int manaCost = (int)(100 * Main.player[Main.myPlayer].manaCost);
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip5")
                    {
                        line.Text = "[c/3a83e4:Transforms Magic attacks into a powerful splitting fireball for " + manaCost + " mana per cast]";
                    }
                }
			}
			if (CalamityWorld.downedSCal)
			{
				if (!CalamityWorld.death)
				{
					foreach (TooltipLine line in tooltips)
					{
						if (line.Mod == "Terraria" && line.Name == "Tooltip11")
						{
							line.Text = "";
						}
					}
				}
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();

            modPlayer.pArtifact = true;
            modPlayer.profanedCrystal = true;

            if (hideVisual)
                modPlayer.profanedCrystalHide = true;
        }
        
        public static void MakeRecipesCheaper(Recipe recipe, int type, ref int numRequired)
        {
            int shadowSpec = ModContent.ItemType<ShadowspecBar>();
            int geode = ModContent.ItemType<DivineGeode>();
            int essence = ModContent.ItemType<UnholyEssence>();
            bool biomePower = Main.LocalPlayer.ZoneHallow || Main.LocalPlayer.ZoneUnderworldHeight;
            numRequired = biomePower && (type == (shadowSpec | geode | essence)) ? numRequired / 2 : numRequired; //cuts the above mats consumed by half if in the biomes instead of arbitrary biome locking
        }   

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ShadowspecBar>(), 10)
            .AddIngredient(ModContent.ItemType<CoreofCinder>(), 5)
            .AddIngredient(ModContent.ItemType<UeliaceBar>(), 25)
            .AddIngredient(ModContent.ItemType<ProfanedSoulArtifact>())
            .AddIngredient(ModContent.ItemType<DivineGeode>(), 50)
            .AddIngredient(ModContent.ItemType<UnholyEssence>(), 100)
            .AddIngredient(ItemID.ObsidianRose)
            .AddTile(ModContent.TileType<ProfanedBasin>())
            .AddConsumeItemCallback(MakeRecipesCheaper)
            .Register();
        }
    }
}
