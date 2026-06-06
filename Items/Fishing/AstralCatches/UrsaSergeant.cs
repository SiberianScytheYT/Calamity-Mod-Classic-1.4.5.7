using CalRD.Buffs.DamageOverTime;
using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.AstralCatches
{
    public class UrsaSergeant : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ursa Sergeant");
/*
            Tooltip.SetDefault("+20 defense but 35% reduced movement speed\n" +
                "Immune to Astral Infection and Feral Bite\n" +
                "Increased regeneration at lower health");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 26;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = 8;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.ursaSergeant = true;
            player.statDefense += 20;
            player.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            player.buffImmune[BuffID.Rabies] = true; //Feral Bite
        }
    }
}
