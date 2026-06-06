using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.CalPlayer;
using CalRD.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Accessories
{
	[AutoloadEquip(EquipType.Shield)]
    public class AsgardianAegis : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Asgardian Aegis");
/*
            Tooltip.SetDefault("Grants immunity to fire blocks and knockback\n" +
                "Immune to most debuffs\n" +
                "+40 max life\n" +
                "Grants a supreme holy flame dash\n" +
                "Can be used to ram enemies\n" +
                "TOOLTIP LINE HERE\n" +
                "Activating this buff will reduce your movement speed and increase enemy aggro\n" +
                "10% damage reduction while submerged in liquid");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 54;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.defense = 10;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            string hotkey = CalRD.AegisHotKey.TooltipHotkeyString();
            foreach (TooltipLine line2 in list)
            {
                if (line2.Mod == "Terraria" && line2.Name == "Tooltip5")
                {
                    line2.Text = "Press " + hotkey + " to activate buffs to all damage, crit chance, and defense";
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
			modPlayer.dashMod = 4;
            modPlayer.elysianAegis = true;
            player.noKnockback = true;
            player.fireWalk = true;
            player.statLifeMax2 += 40;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
            player.buffImmune[ModContent.BuffType<HolyFlames>()] = true;
            player.buffImmune[ModContent.BuffType<GlacialState>()] = true;
            player.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = true;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            { player.endurance += 0.1f; }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AsgardsValor>());
            recipe.AddIngredient(ModContent.ItemType<ElysianAegis>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
