using CalRD.CalPlayer;
using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
    public class AuricTeslaWireHemmedVisage : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Auric Tesla Wire-Hemmed Visage");
/*
            Tooltip.SetDefault("20% increased magic damage and critical strike chance, +100 max mana, and 20% reduced mana usage\n" +
                               "Not moving boosts all damage and critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.defense = 24; //132
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AuricTeslaBodyArmor>() && legs.type == ModContent.ItemType<AuricTeslaCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Magic Tarragon, Bloodflare, God Slayer, and Silva armor effects\n" +
                "All projectiles spawn healing auric orbs on enemy hits\n" +
                "Max run speed and acceleration boosted by 10%";
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.tarraMage = true;
            modPlayer.bloodflareSet = true;
            modPlayer.bloodflareMage = true;
            modPlayer.godSlayer = true;
            modPlayer.godSlayerMage = true;
            modPlayer.silvaSet = true;
            modPlayer.silvaMage = true;
            modPlayer.auricSet = true;
            player.thorns += 3f;
            player.lavaMax += 240;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            if (player.lavaWet)
            {
                player.statDefense += 30;
                player.lifeRegen += 10;
            }
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.auricBoost = true;
            player.manaCost *= 0.8f;
            player.GetDamage(DamageClass.Magic) += 0.2f;
            player.GetCritChance(DamageClass.Magic) += 20;
            player.statManaMax2 += 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SilvaMaskedCap>());
            recipe.AddIngredient(ModContent.ItemType<GodSlayerVisage>());
            recipe.AddIngredient(ModContent.ItemType<BloodflareHornedMask>());
            recipe.AddIngredient(ModContent.ItemType<TarragonMask>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<PsychoticAmulet>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
