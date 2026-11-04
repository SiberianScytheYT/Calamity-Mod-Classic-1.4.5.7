using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AerospecHeadgear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aerospec Headgear");
/*
            Tooltip.SetDefault("8% increased rogue damage and 5% increased movement speed");
*/
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = 3;
            Item.defense = 4; //17
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AerospecBreastplate>() && legs.type == ModContent.ItemType<AerospecLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = CalamityUtils.GetTextValue("Misc.CalamitySetBonus.AerospecHeadgear");
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.aeroSet = true;
            modPlayer.rogueStealthMax += 1f;
            player.noFallDmg = true;
            player.moveSpeed += 0.05f;
            player.Calamity().throwingCrit += 5;
            player.Calamity().wearingRogueArmor = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().throwingDamage += 0.08f;
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 5);
            recipe.AddIngredient(ItemID.Cloud, 3);
            recipe.AddIngredient(ItemID.RainCloud);
            recipe.AddIngredient(ItemID.Feather);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }
    }
}
