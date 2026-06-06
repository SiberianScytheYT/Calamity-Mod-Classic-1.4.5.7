using CalRD.Items.Materials;
using CalRD.NPCs.ProfanedGuardians;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class ProfanedShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Profaned Shard");
/*
            Tooltip.SetDefault("A shard of the unholy flame\n" +
                "Summons the Profaned Guardians\n" +
                "Can only be used during daytime");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<ProfanedGuardianBoss>()) && Main.dayTime && (player.ZoneHallow || player.ZoneUnderworldHeight);
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ProfanedGuardianBoss>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<ProfanedGuardianBoss>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UnholyEssence>(), 15);
            recipe.AddIngredient(ItemID.LunarBar, 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
