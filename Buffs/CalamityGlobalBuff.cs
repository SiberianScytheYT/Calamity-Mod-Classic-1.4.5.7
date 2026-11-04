using CalRD.Buffs.StatDebuffs;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs
{
	public class CalamityGlobalBuff : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (type == BuffID.Shine)
            {
                player.Calamity().shine = true;
            }
            else if (type == BuffID.IceBarrier)
            {
                player.endurance -= 0.1f;
            }
            else if (type == BuffID.ObsidianSkin)
            {
                player.lavaMax += 420;
            }
            else if (type >= BuffID.NebulaUpDmg1 && type <= BuffID.NebulaUpDmg3)
            {
                float nebulaDamage = 0.075f * player.nebulaLevelDamage; //7.5% to 22.5%
                player.GetDamage(DamageClass.Generic) -= nebulaDamage;
            }
            else if (type >= BuffID.NebulaUpLife1 && type <= BuffID.NebulaUpLife3)
            {
                player.lifeRegen -= 5 * player.nebulaLevelLife; //10 to 30 changed to 5 to 15
            }
            else if (type == BuffID.Warmth)
            {
                player.buffImmune[ModContent.BuffType<GlacialState>()] = true;
                player.buffImmune[BuffID.Frozen] = true;
                player.buffImmune[BuffID.Chilled] = true;
            }
        }

        public override void Update(int type, NPC npc, ref int buffIndex)
        {
			if (type == BuffID.Webbed)
			{
				if (npc.Calamity().webbed < npc.buffTime[buffIndex])
					npc.Calamity().webbed = npc.buffTime[buffIndex];
				npc.DelBuff(buffIndex);
				buffIndex--;
			}
            else if (type == BuffID.Slow)
            {
                if (npc.Calamity().slowed < npc.buffTime[buffIndex])
                    npc.Calamity().slowed = npc.buffTime[buffIndex];
                npc.DelBuff(buffIndex);
                buffIndex--;
            }
            if (type == BuffID.Electrified)
            {
                if (npc.Calamity().electrified < npc.buffTime[buffIndex])
                    npc.Calamity().electrified = npc.buffTime[buffIndex];
                npc.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
        {
            Player player = Main.player[Main.myPlayer];

			//Vanilla buffs
            switch (type)
            {
                case BuffID.NebulaUpDmg1:
                    tip = tip.Replace("15", "7.5");
                    break;

                case BuffID.NebulaUpDmg2:
                    tip = tip.Replace("30", "15");
                    break;

                case BuffID.NebulaUpDmg3:
                    tip = tip.Replace("45", "22.5");
                    break;

                case BuffID.BeetleEndurance1:
                    tip = tip.Replace("15", "10");
                    break;

                case BuffID.BeetleEndurance2:
                    tip = tip.Replace("30", "20");
                    break;

                case BuffID.BeetleEndurance3:
                    tip = tip.Replace("45", "30");
                    break;

                case BuffID.WeaponImbueVenom:
                case BuffID.WeaponImbueCursedFlames:
                case BuffID.WeaponImbueFire:
                case BuffID.WeaponImbueGold:
                case BuffID.WeaponImbueIchor:
                case BuffID.WeaponImbueNanites:
                case BuffID.WeaponImbuePoison:
                    tip = CalamityUtils.GetTextValue("Vanilla.BuffDescription.WeaponImbueRogue") + tip;
                    break;

                case BuffID.WeaponImbueConfetti:
                    tip = CalamityUtils.GetTextValue("Vanilla.BuffDescription.WeaponImbueConfetti");
                    break;

                case BuffID.IceBarrier:
                    tip = tip.Replace("25", "15");
                    break;

                case BuffID.ChaosState:
                    tip = CalamityUtils.GetTextValue("Vanilla.BuffDescription.ChaosState");
                    break;

                case BuffID.CursedInferno:
					if (CalamityWorld.revenge)
						tip += ". " + CalamityUtils.GetTextValue("Vanilla.BuffDescription.CursedInfernoExtra");
                    break;

                case BuffID.Warmth:
                    tip += ". " + CalamityUtils.GetTextValue("Vanilla.BuffDescription.WarmthExtra");
					if (CalamityWorld.death)
						tip += ". " + CalamityUtils.GetTextValue("Misc.ColdProtection");
                    break;

                case BuffID.Invisibility:
                    tip += ". " + CalamityUtils.GetTextValue("Vanilla.BuffDescription.InvisibilityExtra");
                    break;

                case BuffID.ObsidianSkin:
					if (CalamityWorld.death)
						tip += ". " + CalamityUtils.GetTextValue("Misc.HeatProtection");
                    break;

                case BuffID.Inferno:
					if (CalamityWorld.death)
						tip += ". " + CalamityUtils.GetTextValue("Misc.ColdProtection");
                    break;

                case BuffID.Campfire:
					if (CalamityWorld.death)
						tip += ". " + CalamityUtils.GetTextValue("Misc.ColdProtection");
                    break;

                case BuffID.Daybreak:
                    tip = CalamityUtils.GetTextValue("Vanilla.BuffDescription.DaybreakExtra");
                    break;
            }
        }
    }
}
