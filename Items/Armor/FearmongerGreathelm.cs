using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FearmongerGreathelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fearmonger Greathelm");
/*
            Tooltip.SetDefault("Pure terror radiates from your eyes\n" +
			"+60 max mana and 10% decreased mana usage\n" +
			"10% increased minion damage and +2 max minions");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 75);
            Item.defense = 38; // 132 total
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 60;
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Summon) += 0.1f;
            player.manaCost *= 0.9f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FearmongerPlateMail>() && legs.type == ModContent.ItemType<FearmongerGreaves>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
			if (CalamityWorld.death)
			{
				player.setBonus = @"30% increased minion damage
The minion damage nerf while wielding weaponry is reduced
Immunity to all forms of frost and flame
All minion attacks grant colossal life regeneration
15% increased damage reduction during the Pumpkin and Frost Moons
This extra damage reduction ignores the soft cap
Provides cold protection in Death Mode";
			}
			else
			{
				player.setBonus = @"30% increased minion damage
The minion damage nerf while wielding weaponry is reduced
Immunity to all forms of frost and flame
All minion attacks grant colossal life regeneration
15% increased damage reduction during the Pumpkin and Frost Moons
This extra damage reduction ignores the soft cap";
			}

            // This bool encompasses cross-class nerf immunity, colossal life regen on minion attack, and the holiday moon DR
            player.Calamity().fearmongerSet = true;

            // All-class armors count as rogue sets, but don't grant stealth bonuses
            player.Calamity().wearingRogueArmor = true;
            player.GetDamage(DamageClass.Summon) += 0.3f;

            int[] immuneDebuffs = {
                BuffID.OnFire,
                BuffID.Frostburn,
                BuffID.CursedInferno,
                BuffID.ShadowFlame, //doesn't do anything
                BuffID.Daybreak, //doesn't do anything
                BuffID.Burning,
                ModContent.BuffType<Shadowflame>(),
                ModContent.BuffType<BrimstoneFlames>(),
                ModContent.BuffType<AbyssalFlames>(),
                ModContent.BuffType<HolyFlames>(),
                ModContent.BuffType<GodSlayerInferno>(),
                BuffID.Chilled,
                BuffID.Frozen,
                ModContent.BuffType<GlacialState>(),
            };
            for (int i = 0; i < immuneDebuffs.Length; ++i)
                player.buffImmune[immuneDebuffs[i]] = true;

            // Constantly emit dim orange light
            Lighting.AddLight(player.Center, 0.3f, 0.18f, 0f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpookyHelmet);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2);
            recipe.AddIngredient(ItemID.SoulofFright, 8);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
