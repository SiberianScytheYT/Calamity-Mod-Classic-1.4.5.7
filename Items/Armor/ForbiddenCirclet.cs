using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ForbiddenCirclet : ModItem
    {
		public const int manaCost = 60;
		public const int tornadoBaseDmg = 80;
		public const float tornadoBaseKB = 1f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Forbidden Circlet");
/*
            Tooltip.SetDefault("10% increased summon damage and 15% increased rogue velocity");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 1; //21
			//same rarity and sell price as Forbidden Mask
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.rare = 5;
			//I lied, it's patron dark red
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.AncientBattleArmorShirt && legs.type == ItemID.AncientBattleArmorPants;
        }

        public override void ArmorSetShadows(Player player)
        {
			player.armorEffectDrawShadowLokis = true;
            player.armorEffectDrawOutlinesForbidden = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            int stormMana = (int)(manaCost * player.manaCost);
            string hotkey = CalRD.TarraHotKey.TooltipHotkeyString();
            player.setBonus = CalamityUtils.GetText("Misc.CalamitySetBonus.ForbiddenCirclet").Format(hotkey, stormMana);
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.forbiddenCirclet = true;
            modPlayer.rogueStealthMax += 0.4f;
            modPlayer.wearingRogueArmor = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.1f;
			player.Calamity().throwingVelocity += 0.15f;
        }

        public override void AddRecipes()
        {
			//Same recipe as Forbidden Mask
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe.AddRecipeGroup("AnyAdamantiteBar", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
