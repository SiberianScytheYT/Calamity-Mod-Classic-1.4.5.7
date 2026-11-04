using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DaedalusVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Daedalus Facemask");
/*
            Tooltip.SetDefault("13% increased rogue damage and 7% increased rogue critical strike chance, increases rogue velocity by 15%\n" +
                "17% increased movement speed");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = 5;
            Item.defense = 7; //37
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DaedalusBreastplate>() && legs.type == ModContent.ItemType<DaedalusLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.DaedalusVisor");
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.daedalusSplit = true;
            modPlayer.rogueStealthMax += 1.05f;
            modPlayer.throwingDamage += 0.05f;
            modPlayer.wearingRogueArmor = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingVelocity += 0.15f;
            player.Calamity().throwingDamage += 0.13f;
            player.Calamity().throwingCrit += 7;
            player.moveSpeed += 0.17f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 8);
			recipe.AddIngredient(ItemID.CrystalShard, 6);
			recipe.AddIngredient(ModContent.ItemType<EssenceofEleum>());
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
