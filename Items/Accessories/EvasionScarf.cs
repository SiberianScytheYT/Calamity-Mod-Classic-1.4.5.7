using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class EvasionScarf : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Evasion Scarf");
/*
            Tooltip.SetDefault("True melee strikes deal 25% more damage\n" +
                "Grants the ability to dash; dashing into an attack will cause you to dodge it\n" +
                "After a dodge you will be granted a buff to all damage, melee speed, and all crit chance for a short time\n" +
                "After a successful dodge you must wait 13 seconds before you can dodge again\n" +
                "This cooldown will be 50 percent longer if you have Chaos State\n" +
                "While on cooldown, Chaos State will be 50 percent longer");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */ => !player.Calamity().dodgeScarf;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.dodgeScarf = true;
            modPlayer.evasionScarf = true;
            modPlayer.dashMod = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CounterScarf>());
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
