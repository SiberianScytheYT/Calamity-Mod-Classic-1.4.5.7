using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerVisage : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("God Slayer Visage");
/*
            Tooltip.SetDefault("14% increased magic damage and critical strike chance\n" +
                "+100 max mana and 17% reduced mana usage");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 75, 0, 0);
            Item.defense = 21; //96
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<GodSlayerChestplate>() && legs.type == ModContent.ItemType<GodSlayerLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.godSlayer = true;
            modPlayer.godSlayerMage = true;
            player.setBonus = "You will survive fatal damage and will be healed 150 HP if an attack would have killed you\n" +
                "This effect can only occur once every 45 seconds\n" +
                "While the cooldown for this effect is active all life regen is halved\n" +
                "Enemies will release god slayer flames and healing flames when hit with magic attacks\n" +
                "Taking damage will cause you to release a magical god slayer explosion";
        }

        public override void UpdateEquip(Player player)
        {
            player.manaCost *= 0.83f;
            player.GetDamage(DamageClass.Magic) += 0.14f;
            player.GetCritChance(DamageClass.Magic) += 14;
            player.statManaMax2 += 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 14);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
