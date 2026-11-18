using CalRD.CalPlayer;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Summon
{
	public class ProfanedBabs : ModBuff //Buff name is reference to how I refer to the guardians as my babs ~Amber
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Profaned Soul");
            // Description.SetDefault("The healer will heal your wounds!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (!modPlayer.gHealer || modPlayer.profanedCrystalBuffs)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Player player = Main.player[Main.myPlayer];
			bool offense = player.Calamity().gOffense;
			bool defense = player.Calamity().gDefense;
			if (player.Calamity().profanedCrystal && (!player.Calamity().profanedCrystalBuffs && !CalamityWorld.downedSCal))
			{
				tip = this.GetLocalizedValue("VanityDescription");
			}
			else if (offense && defense)
			{
				tip = this.GetLocalizedValue("AllActive");
			}
			else if (offense || defense)
			{
				tip = this.GetLocalization("EitherActive").Format(this.GetLocalizedValue(offense ? "Offense" : "Defense"), this.GetLocalizedValue(offense ? "OffenseDescription" : "DefenseDescription"));
			}
		}
    }
}
