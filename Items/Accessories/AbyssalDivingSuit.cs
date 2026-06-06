using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class AbyssalDivingSuit : ModItem
    {
        public override void Load()
        {
            // this code wasn't here originally but it's here out of necessity, what with having to load these assets without an implicit path to them
            if (Main.netMode != NetmodeID.Server)
            {
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/AbyssalDivingSuit_Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/AbyssalDivingSuit_Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Accessories/AbyssalDivingSuit_Legs", EquipType.Legs, this);
            }
        }
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Diving Suit");
/*
            Tooltip.SetDefault("Transforms the holder into an armored diver\n" +
                "Increases movement speed while underwater and moves slowly outside of water\n" +
                "The suits' armored plates reduce damage taken by 15%\n" +
                "The plates will only take damage if the damage taken is over 50\n" +
                "After the suit has taken too much damage its armored plates will take 3 minutes to regenerate\n" +
                "Reduces the damage caused by the pressure of the abyss while out of breath\n" +
                "Removes the bleed effect caused by the abyss in all layers except the deepest one\n" +
                "Grants the ability to swim and greatly extends underwater breathing\n" +
                "Provides light underwater and extra mobility on ice\n" +
                "Provides a moderate amount of light in the abyss\n" +
                "Greatly reduces breath loss in the abyss\n" +
                "Reduces creature's ability to detect you in the abyss\n" +
                "Reduces the defense reduction that the abyss causes\n" +
                "Allows you to fall faster while in liquids");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = 10;
			Item.Calamity().postMoonLordRarity = 12;
            if (Main.netMode != NetmodeID.Server)
            {
                int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
                int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
                int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
                ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
                ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
                ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
                ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.abyssalDivingSuit = true;
            if (hideVisual)
            {
                modPlayer.abyssalDivingSuitHide = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AbyssalDivingGear>());
            recipe.AddIngredient(ModContent.ItemType<AnechoicPlating>());
            recipe.AddIngredient(ModContent.ItemType<IronBoots>());
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 40);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 40);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 15);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
