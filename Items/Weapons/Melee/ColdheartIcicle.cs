using CalRD.NPCs.Providence;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Melee
{
    public class ColdheartIcicle : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coldheart Icicle");
/*
            Tooltip.SetDefault("Drains a percentage of enemy health on hit\n"
                               +"Cannot inflict critical hits");
*/
        }
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 26;
            Item.height = 26;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
        }

        public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
        {
            Item.damage = target.statLifeMax2 * 2 / 100;
            target.statDefense -= target.statDefense;
            target.endurance = 0f;
            // crit = false;
        }

        // LATER -- Providence specifically is immune to Coldheart Icicle. There is probably a better way to do this
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            Item.damage = 1;
            modifiers.DisableCrit();
            if (target.type != NPCID.TargetDummy && target.type != ModContent.NPCType<Providence>())
                target.life -= target.lifeMax * 2 / 100;
            target.checkDead();
        }
    }
}
