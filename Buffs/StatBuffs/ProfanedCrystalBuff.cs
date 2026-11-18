using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.StatBuffs
{
    public class ProfanedCrystalBuff : ModBuff 
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Devotion");
            // Description.SetDefault("The profaned soul within has been defiled by the powerful magic of a supreme witch");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true; ///* tModPorter Note: Removed. Use BuffID.Sets.NurseCannotRemoveDebuff instead, and invert the logic */ = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (CalamityWorld.downedSCal && (player.Calamity().minionSlotStat - player.slotsMinions) >= 10 && !player.Calamity().profanedCrystalForce)
            {
                player.Calamity().profanedCrystalBuffs = true;
            }
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Player player = Main.player[Main.myPlayer];
			if (player.Calamity().profanedCrystalBuffs)
			{
				if (player.Calamity().endoCooper)
				{
					tip = this.GetLocalizedValue("NerfedEndo");
				}
				else if (player.Calamity().magicHat)
				{
					tip = this.GetLocalizedValue("NerfedHat");
				}
				else
				{
					bool offense = (Main.dayTime && !player.wet) || player.lavaWet;
					tip += "\n" + (offense ? this.GetLocalization("Offense").Format(this.GetLocalizedValue(player.lavaWet ? "Lava" : "Day"))
						: this.GetLocalization("Defense").Format(this.GetLocalizedValue(player.honeyWet ? "Honey" : (player.wet ? "Water" : "Night"))));

					if (player.statLife <= (int)(player.statLifeMax2 * 0.5))
						tip += "\n" + this.GetLocalizedValue("Enrage"); 
				}
			}
			else if (CalamityWorld.downedSCal)
			{
				tip = this.GetLocalizedValue("LockedSlots");
			}
			else
			{
				tip = this.GetLocalizedValue("LockedSCal");
			}
		}
    }
}
