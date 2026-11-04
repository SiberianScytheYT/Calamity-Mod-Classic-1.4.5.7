using CalRD.Buffs.Cooldowns;
using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class BrimflameScowl : ModItem
    {
        private bool frenzy = false;
        public static int CooldownLength = 1800;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimflame Cowl");
/*
            Tooltip.SetDefault("5% increased magic damage and critical strike chance\n" +
                "Increases maximum mana by 70 and reduces mana usage by 10%\n" +
                "Immunity to On Fire!, Brimstone Flames and Frostburn");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.defense = 7; //41
        }

        private void updateFrenzy(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (!frenzy)
            {
                if (modPlayer.brimflameFrenzy)
                    frenzy = true;
            }
            else
            {
                if (!modPlayer.brimflameFrenzy)
                {
                    frenzy = false;
                    player.AddBuff(ModContent.BuffType<BrimflameFrenzyCooldown>(), CooldownLength, true);
					modPlayer.brimflameFrenzyTimer = CooldownLength;
                }
            }
			if (modPlayer.brimflameFrenzyTimer == 1) //sound when ready to use again
			{
				SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/BrimflameRecharge"), player.Center);
			}
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.statManaMax2 += 70;
            player.manaCost *= 0.9f;
            player.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            updateFrenzy(player);
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BrimflameRobes>() && legs.type == ModContent.ItemType<BrimflameBoots>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.brimflameSet = true;
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.GetCritChance(DamageClass.Magic) += 15;
            string hotkey = CalRD.TarraHotKey.TooltipHotkeyString();
            player.setBonus = CalamityUtils.GetText("Misc.CalamitySetBonus.BrimflameScowl").Format(hotkey);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CalamityDust>(), 10);
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
