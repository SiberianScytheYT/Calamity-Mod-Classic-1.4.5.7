using CalRD.CalPlayer;
using CalRD.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AstralHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Helm");
/*
            Tooltip.SetDefault("Danger detection");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 40, 0, 0);
            Item.rare = 9;
            Item.defense = 17; //63
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AstralBreastplate>() && legs.type == ModContent.ItemType<AstralLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "25% increased movement speed\n" +
                "28% increased damage and 21% increased critical strike chance\n" +
                "Whenever you crit an enemy fallen, hallowed, and astral stars will rain down\n" +
                "This effect has a 1 second cooldown before it can trigger again";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.astralStarRain = true;
            player.moveSpeed += 0.25f;
            player.GetDamage(DamageClass.Generic) += 0.28f;
            modPlayer.AllCritBoost(21);
            player.Calamity().wearingRogueArmor = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.dangerSense = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 8);
			recipe.AddIngredient(ItemID.MeteoriteBar, 6);
			recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
