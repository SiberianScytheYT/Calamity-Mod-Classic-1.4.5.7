using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AerospecHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aerospec Helm");
/*
            Tooltip.SetDefault("8% increased melee damage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = 3;
            Item.defense = 7; //20
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
            player.setBonus = "5% increased movement speed and melee critical strike chance\n" +
                    "Taking over 25 damage in one hit will cause a spread of homing feathers to fall\n" +
                    "Allows you to fall more quickly and disables fall damage";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.aeroSet = true;
            player.noFallDmg = true;
            player.moveSpeed += 0.05f;
            player.GetCritChance(DamageClass.Melee) += 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.08f;
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
