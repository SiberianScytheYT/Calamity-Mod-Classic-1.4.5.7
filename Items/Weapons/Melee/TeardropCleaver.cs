using CalRD.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TeardropCleaver : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Teardrop Cleaver");
/*
            Tooltip.SetDefault("Makes your enemies cry");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.damage = 18;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.useTurn = true;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TemporalSadness>(), 120);
        }
    }
}
