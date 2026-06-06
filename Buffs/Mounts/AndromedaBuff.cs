using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Buffs.Mounts
{
	public class AndromedaBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Andromeda");
            // Description.SetDefault("You're controlling a piece of history");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
			//Disables crouching with the Crouch, Crawl, and Roll Mod
            ModLoader.TryGetMod("CrouchMod", out Mod crouchMod);
            if (crouchMod != null)
            {
				//Mod Call inputs
				//"CanCrouch"   //string which is required
				//p             //int for the player's index
				//canCrouch     //bool whether the player can input crouch (point being to set this to false)
				//forceUnCrouch //bool which, if true, forces the player to uncrouch (forcing their head into the ceiling)
                crouchMod.Call("CanCrouch", player.whoAmI, false, true);
            }
        }
    }
}
