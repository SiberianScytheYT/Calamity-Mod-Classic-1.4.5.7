using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaver Headgear");
/*
            Tooltip.SetDefault("15% increased rogue damage, 5% increased rogue velocity and critical strike chance\n" +
                "20% increased movement speed and can move freely through liquids");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 7;
            Item.defense = 10; //43
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ReaverScaleMail>() && legs.type == ModContent.ItemType<ReaverCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.reaverSpore = true;
            modPlayer.rogueStealthMax += 1.1f;
            player.setBonus = "5% increased rogue damage\n" +
                "You emit a cloud of spores when you are hit\n" +
                "Rogue stealth builds while not attacking and slower while moving, up to a max of 110\n" +
                "Once you have built max stealth, you will be able to perform a Stealth Strike\n" +
                "Rogue stealth only reduces when you attack, it does not reduce while moving\n" +
                "The higher your rogue stealth the higher your rogue damage, crit, and movement speed";
            player.Calamity().throwingDamage += 0.05f;
            player.Calamity().wearingRogueArmor = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.Calamity().throwingDamage += 0.15f;
            player.Calamity().throwingCrit += 5;
            player.Calamity().throwingVelocity += 0.05f;
            player.moveSpeed += 0.2f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 8);
			recipe.AddIngredient(ItemID.JungleSpores, 6);
			recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>());
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
